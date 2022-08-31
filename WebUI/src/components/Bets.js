import React, {Component} from 'react';
import {Button, Modal, OverlayTrigger, Popover, Table} from "react-bootstrap";
import {createBet, getAllOptions, getBetPreview} from "../services/betService";
import {toast, ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import BetsTable from "./BetsTable";
import 'bootstrap-icons/font/bootstrap-icons.css';


class Bets extends Component {
    constructor(props) {
        super(props);

        this.state = {
            showModal: false,
            options: [],
            previewBet: {options: [{optionInfos: []}]},
            payout: this.props.defaultPayout,
            expirationDate: 0
        };

        this.payoutInput = React.createRef();

        this.popover = (
            <Popover id="popover-basic">
                <Popover.Header as="h3">Betting Info</Popover.Header>
                <Popover.Body>
                    You can bet with minimum of $10 and maximum of $1000.
                    Bet if you think that after the expiration time the price will
                    go higher or lower. You pay with the payout that you give and if
                    your prediction is right, you get your money back.
                    Whether you win or lose you get percentage bonus which is based on the payout
                    you selected.
                </Popover.Body>
            </Popover>
        );
        
        this.updateOptions = this.updateOptions.bind(this);
        this.completeOptionHandle = this.completeOptionHandle.bind(this);
    }

    handleClose = () => this.setState({showModal: false});
    handleShow = () => this.setState({showModal: true});

    handlePayoutChange(event) {
        let payout;
        const regex = /^[0-9]*$/;
        
        if (regex.test(event.target.value) && event.target.value <= 1000) {
            payout = event.target.value;
        } else {
            payout = this.state.payout;
        }

        this.setState({payout});
    }

    handleExpirationDateChange(event) {
        this.setState({expirationDate: event.target.value});
    }

    componentDidMount() {
        this.updateBetPreview();
        this.updateOptions();
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.currencyValue !== this.props.currencyValue) {
            this.updateBetPreview();
        }
    }

    updateBetPreview() {
        console.log('Bets:updateBetPreview')
        getBetPreview(this.props.token, this.props.currency)
            .then(data => {
                this.setState({previewBet: data})
            })
    }

    updateOptions() {
        console.log('Bets:updateOptions')
        getAllOptions(this.props.token)
            .then(response => {
                this.setState({options: response})
            });
    }
    
    completeOptionHandle() {
        this.updateOptions();
        this.props.updateUser();
    }

    createBet(barrier, slope, percentageReward) {
        let payout = this.payoutInput.current.value
        let expirationDateIndex = this.state.expirationDate;

        if (payout <= 0 || !['1', '2', '3'].includes(expirationDateIndex)) {
            return;
        }

        let bet = {
            currency: this.props.currency,
            payout: payout,
            barrier: barrier,
            slope: slope,
            percentageReward: percentageReward,
            expirationDate: this.state.previewBet.options[expirationDateIndex - 1].expirationDate
        }

        createBet(this.props.token, bet)
            .then(response => {
                let options = {
                    position: "bottom-right",
                    autoClose: 5000,
                    hideProgressBar: false,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true,
                    progress: undefined,
                };
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
            })
            .then(() => {
                this.updateOptions();
                this.props.updateUser();
            });
    }


    render() {
        return (
            <>
                <Button className='m-3' variant="outline-success" onClick={this.handleShow}>
                    Make a Bet
                </Button>

                <Modal show={this.state.showModal} onHide={this.handleClose} centered>
                    <Modal.Header closeButton>
                        <Modal.Title>Make a Bet</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>

                        <div className="input-group mb-3">
                            <span className="input-group-text">Balance:
                                <h5 className='mx-3'>
                                    <span className="badge bg-light text-dark lead">${this.props.user.balance}</span>
                                </h5>
                            </span>
                            <span className="input-group-text">Selected Currency:
                                <h5 className='mx-3'>
                                    <span className="badge bg-light text-dark lead">{this.props.currency}</span>
                                </h5>
                            </span>
                        </div>

                        <div className="input-group mb-3">
                            <select className="form-select" onChange={this.handleExpirationDateChange.bind(this)}
                                    value={this.state.expirationDate}>
                                <option value='0'>Choose Expiration</option>
                                <option value="1">10 Minutes</option>
                                <option value="2">30 Minutes</option>
                                <option value="3">1 Hour</option>
                            </select>
                            <span className="input-group-text">$</span>
                            <input ref={this.payoutInput} type="text" className="form-control" placeholder='Payout'
                                   onInput={this.handlePayoutChange.bind(this)} value={this.state.payout}/>
                            <OverlayTrigger trigger="click" placement="right" overlay={this.popover}>
                                <Button><i className="bi bi-info-circle"></i></Button>
                            </OverlayTrigger>
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
                                this.state.previewBet.options[0].optionInfos.map(option =>
                                    <tr className='row' key={option.barrier}>
                                        <td className='col-3 text-center align-middle'>
                                            <h4><span className="badge bg-light text-dark lead">{option.barrier}</span>
                                            </h4>
                                        </td>
                                        <td className='col-4 d-grid gap-2'>
                                            <button className='btn btn-info row'
                                                    onClick={() => this.createBet(option.barrier, 'higher', option.percentageRewards.higher)}>
                                                {option.percentageRewards.higher * 100}%
                                            </button>
                                        </td>
                                        <td className='col-4 mx-auto d-grid gap-2'
                                            onClick={() => this.createBet(option.barrier, 'lower', option.percentageRewards.lower)}>
                                            <button className='btn btn-warning row'>
                                                {option.percentageRewards.lower * 100}%
                                            </button>
                                        </td>
                                    </tr>
                                )
                            }
                            </tbody>
                        </Table>
                    </Modal.Body>
                </Modal>

                <BetsTable token={this.props.token} options={this.state.options} 
                           onComplete={this.completeOptionHandle}>
                </BetsTable>

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
    
    
    
    

