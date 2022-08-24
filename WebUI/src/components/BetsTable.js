import React, {Component} from 'react';
import BootstrapTable from "react-bootstrap-table-next";
import paginationFactory from "react-bootstrap-table2-paginator";

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
    }];

    defaultSorted = [{
        dataField: 'startDate',
        order: 'desc'
    }];

    rowStyle(row, index) {
        let style = {
            backgroundColor: '#e6f7ff'
        };
        
        if (!row.isActive) {
            style.backgroundColor = '#ffffe6';
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
                    expirationDate: new Date(x.expirationDate).toLocaleString(),
                    currency: x.currency,
                    payout: x.payout,
                    slope: x.slope,
                    barrier: x.barrier,
                    percentageReward: x.percentageReward,
                    isActive: x.isActive
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