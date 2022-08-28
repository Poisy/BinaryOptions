import React, {Component} from 'react';
import {Button, Modal} from "react-bootstrap";

class Profile extends Component {
    constructor(props) {
        super(props);
        
        this.state = {
            showModal: false
        }
        
        this.handleShowModal = this.handleShowModal.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
    }
    
    handleShowModal() { this.setState({ showModal: true }) }
    handleCloseModal() { this.setState({ showModal: false }) }
    
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
                        </Modal.Body>
                        <Modal.Footer>
                            
                        </Modal.Footer>
                    </Modal>
                </>
        );
    }
}

export default Profile;
