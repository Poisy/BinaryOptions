import React, {Component} from 'react';
import BootstrapTable from "react-bootstrap-table-next";
import paginationFactory from "react-bootstrap-table2-paginator";
import Countdown from "react-countdown-now";

require('react-bootstrap-table-next/dist/react-bootstrap-table2.min.css');
require('react-bootstrap-table2-paginator/dist/react-bootstrap-table2-paginator.min.css');

class BetsTable extends Component {
    constructor(props) {
        super(props);
    }

    columns = [{
        dataField: 'startDate',
        text: 'Start Date',
        sort: true
    }, {
        dataField: 'expirationDate',
        text: 'Expiration Date',
        sort: true
    }, {
        dataField: 'currency',
        text: 'Currency Name',
        sort: true
    }, {
        dataField: 'payout',
        text: 'Payout',
        sort: true
    }, {
        dataField: 'slope',
        text: 'Slope',
        sort: true
    }, {
        dataField: 'barrier',
        text: 'Barrier',
        sort: true
    }, {
        dataField: 'percentageReward',
        text: 'Percentage Reward',
        sort: true
    }, {
        dataField: 'isActive',
        text: 'Is Active?',
        sort: true
    }, {
        dataField: 'didWin',
        text: 'Did Win?',
        sort: true
    }, {
        dataField: 'price',
        text: 'Price',
        sort: true
    }];

    defaultSorted = [{
        dataField: 'startDate',
        order: 'desc'
    }];

    rowStyle(row, index) {
        let style = {
            backgroundColor: '#ffffe6'
        };

        if (row.isActive) {
            style.backgroundColor = '#ffffe6';
        } else if (row.didWin) {
            style.backgroundColor = '#e6ffe6';
        } else {
            style.backgroundColor = '#ffebe6';
        }

        return style;
    }


    render() {
        return (
            <BootstrapTable
                bootstrap4
                keyField="id"
                data={this.props.options.map(x => ({
                    startDate: new Date(x.startDate).toLocaleString(),
                    expirationDate: (
                        !x.isActive ? <p>Expired!</p> :
                        <Countdown date={Date.now() + (new Date(x.expirationDate) - Date.now())}
                                   daysInHours={true}
                                   onComplete={() => this.props.onComplete()}>
                            <p>Expired!</p>
                        </Countdown>),
                    currency: x.currency,
                    payout: x.payout,
                    slope: x.slope,
                    barrier: x.barrier,
                    percentageReward: x.percentageReward,
                    isActive: x['isActive'],
                    didWin: x.didWin,
                    price: x.price
                }))}
                columns={this.columns}
                defaultSorted={this.defaultSorted}
                pagination={paginationFactory()}
                rowStyle={this.rowStyle}
            />
        );
    }
}

export default BetsTable;