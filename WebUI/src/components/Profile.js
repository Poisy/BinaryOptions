import React, {Component} from 'react';
import {Button, Form, Modal} from "react-bootstrap";
import currencies from "../data/currencies";

class Profile extends Component {
    constructor(props) {
        super(props);
        
        this.state = {
            showModal: false,
            currency: this.props.defaultCurrency,
            chartType: this.props.defaultChartType,
            payout: this.props.defaultPayout
        }
        
        this.handleShowModal = this.handleShowModal.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleCurrencyChange = this.handleCurrencyChange.bind(this);
        this.handleChartTypeChange = this.handleChartTypeChange.bind(this);
        this.handlePayoutChange = this.handlePayoutChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    
    handleShowModal() { this.setState({ showModal: true }) }
    handleCloseModal() { this.setState({ showModal: false }) }

    handleCurrencyChange(e) {
        console.log('a'); this.setState({currency: e.target.value}) }
    handleChartTypeChange(e) { this.setState({chartType: e.target.value}) }
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

    handleSubmit() {
        if (this.state.payout < 10) {
            this.setState({ payout: 10 }, () => {
                this.props.updateSettings(this.state.currency, this.state.chartType, this.state.payout);
            })
        } else {
            this.props.updateSettings(this.state.currency, this.state.chartType, this.state.payout);
        }
        
        this.handleCloseModal();
    }

    
    
    render() {
        return (
            this.props.user == null ? "" :
                <>
                    <Button variant="outline-success" onClick={this.handleShowModal}>
                        Profile
                    </Button>

                    <Modal show={this.state.showModal} onHide={this.handleCloseModal}>
                        <Modal.Header closeButton>
                            <Modal.Title>Profile</Modal.Title>
                        </Modal.Header>
                        <Modal.Body className="">
                            <p>Username: {this.props.user.username}</p>
                            <p>Email: {this.props.user.email}</p>
                            <p>Balance: ${this.props.user.balance}</p>
                            <p>Nationality: {this.props.user.nationality}</p>

                            <hr/>
                                
                            <h4 className='my-3'>Settings</h4>
                            
                            <Form.Group className="mb-3" controlId="formBasicEmail">
                                <Form.Label>Default Currency</Form.Label>
                                <Form.Select value={this.state.currency} 
                                             onChange={this.handleCurrencyChange}>
                                    {
                                        currencies.map(currency => (
                                            <option key={currency} value={currency}>{currency}</option>
                                        ))
                                    }
                                </Form.Select>
                            </Form.Group>

                            <Form.Group className="mb-3" controlId="formBasicPassword">
                                <Form.Label>Default Chart Type</Form.Label>
                                <Form.Select value={this.state.chartType.toLowerCase()} 
                                             onChange={this.handleChartTypeChange}>
                                    <option value='candlestick'>Candlestick</option>
                                    <option value='ohlc'>OHLC</option>
                                    <option value='table'>Table</option>
                                </Form.Select>
                            </Form.Group>

                            <Form.Group className="mb-3" controlId="formBasicCheckbox">
                                <Form.Label>Default Payout</Form.Label>
                                <Form.Control type='text' 
                                              onInput={this.handlePayoutChange.bind(this)} 
                                              value={this.state.payout}/>
                            </Form.Group>
                        </Modal.Body>
                        
                        <Modal.Footer>
                            <Button variant="primary" type="submit" onClick={this.handleSubmit}>
                                Save
                            </Button>
                        </Modal.Footer>
                    </Modal>
                </>
        );
    }
}

export default Profile;
