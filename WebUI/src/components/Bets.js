import React, {Component} from 'react';
import {Button, Modal, Table} from "react-bootstrap";
import {createBet, getBetPreview} from "../services/betService";
import {toast, ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';


class Bets extends Component {
    constructor(props) {
        super(props);
        
        this.state = {
            showModal: false,
            previewBet: {},
            data: { options: [ { optionInfos: [] } ] },
            payout: 0,
            expirationDate: 0
        };
        
        this.payoutInput = React.createRef();
    }

    handleClose = () => this.setState({showModal: false});
    handleShow = () => this.setState({showModal: true});
    handlePayoutChange(event) {
        const payout = (event.target.validity.valid) ? event.target.value : this.state.payout;

        this.setState({ payout });
    }
    handleExpirationDateChange(event) {
        this.setState({ expirationDate: event.target.value });
    }
    
    componentDidMount() {
        this.updateOptions();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.currencyValue !== this.props.currencyValue) {
            this.updateOptions();
        }
    }
    
    updateOptions() {
        console.log('Bets:updateOptions')
        getBetPreview(this.props.token, this.props.currency)
            .then(data => {
                this.setState({ data })
            })
    }

    createBet(barrier, slope, percentageReward) {
        let payout = this.payoutInput.current.value
        let expirationDateIndex = this.state.expirationDate;
        
        if (payout <= 0 || !['1', '2', '3'].includes(expirationDateIndex))
        {
            return;
        }
        
        let bet = {
            currency: this.props.currency,
            payout: payout,
            barrier: barrier,
            slope: slope,
            percentageReward: percentageReward,
            expirationDate: this.state.data.options[expirationDateIndex-1].expirationDate
        }
        
        createBet(this.props.token, bet).then(response => {
            console.log(response)
            let options = {
                position: "bottom-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
            };
            console.log(response)
            if (response.status === 200) {
                toast.success('Bet created successfully!', options);
            } else {
                Object.values(response.errors).forEach(errorProp => {
                    errorProp.forEach(error => {
                        toast.error(error, options);
                    })
                })
            }
            this.handleClose();
        });
    }
    
    

    render() {
        return (
            <>
                <Button variant="outline-success" onClick={this.handleShow}>
                    Make a Bet
                </Button>

                <Modal show={this.state.showModal} onHide={this.handleClose} centered>
                    <Modal.Header closeButton>
                        <Modal.Title>Make a Bet</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        
                        <div className="input-group mb-3">
                            <select className="form-select" onChange={this.handleExpirationDateChange.bind(this)} 
                                    value={this.state.expirationDate}>
                                <option value='0'>Choose Expiration</option>
                                <option value="1">10 Minutes</option>
                                <option value="2">30 Minutes</option>
                                <option value="3">1 Hour</option>
                            </select>
                            <span className="input-group-text">$</span>
                            <input ref={this.payoutInput} type="number" className="form-control" placeholder='Payout' pattern="[0-9]*"
                                   onInput={this.handlePayoutChange.bind(this)} value={this.state.payout}/>
                        </div>
                        
                        <Table>
                            <thead className='container'>
                            <tr className='row'>
                                <th className='col-3 text-center'>Barrier</th>
                                <th className='col-4 text-center'>Higher</th>
                                <th className='col-5 text-center'>Lower</th>
                            </tr>
                            </thead>
                            <tbody className='container'>
                            {
                                this.state.data.options[0].optionInfos.map(option =>
                                    <tr className='row' key={option.barrier}>
                                        <td className='col-3 text-center align-middle'>
                                            <h4><span className="badge bg-light text-dark lead">{option.barrier}</span></h4>
                                        </td>
                                        <td className='col-4 d-grid gap-2'>
                                            <button className='btn btn-info row'
                                            onClick={() => this.createBet(option.barrier, 'higher', option.percentageRewards.higher)}>
                                                {option.percentageRewards.higher*100}%
                                            </button>
                                        </td>
                                        <td className='col-4 mx-auto d-grid gap-2'
                                            onClick={() => this.createBet(option.barrier, 'lower', option.percentageRewards.lower)}>
                                            <button className='btn btn-warning row'>
                                                {option.percentageRewards.lower*100}%
                                            </button>
                                        </td>
                                    </tr>
                                )
                            }
                            </tbody>
                        </Table>
                    </Modal.Body>
                </Modal>

                <ToastContainer
                    position="bottom-right"
                    autoClose={5000}
                    hideProgressBar={false}
                    newestOnTop={false}
                    closeOnClick
                    rtl={false}
                    pauseOnFocusLoss
                    draggable
                    pauseOnHover
                />
            </>
        );
    }
}

export default Bets;
    
    
    
    

