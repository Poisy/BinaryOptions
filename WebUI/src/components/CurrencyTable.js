import React, {Component} from 'react';
import BootstrapTable from 'react-bootstrap-table-next';
import paginationFactory from 'react-bootstrap-table2-paginator';
require('react-bootstrap-table-next/dist/react-bootstrap-table2.min.css');
require('react-bootstrap-table2-paginator/dist/react-bootstrap-table2-paginator.min.css');

class CurrencyTable extends Component {
    
    constructor(props) {
        super(props);
    }
    
    columns = [{
        dataField: 'date',
        text: 'Date',
        sort: true
    }, {
        dataField: 'open',
        text: 'Open Price',
        sort: true
    }, {
        dataField: 'high',
        text: 'High Price',
        sort: true
    },
    {
        dataField: 'low',
        text: 'Low Price',
        sort: true
    },
    {
        dataField: 'close',
        text: 'Close Price',
        sort: true
    }];

    defaultSorted = [{
        dataField: 'date',
        order: 'desc'
    }];
    
    rowStyle(row, index) {
        if (row.open > row.close) {
            return { backgroundColor: '#ffe6e6' }
        } else {
            return { backgroundColor: '#ccffcc' }
        }
    }
    

    render() {
        return (
            <BootstrapTable
                bootstrap4
                keyField="id"
                data={this.props.data.map(x => ({ 
                    date: new Date(x[0]).toLocaleString(), 
                    open: x[1],
                    high: x[2],
                    low: x[3],
                    close: x[4]
                }))}
                columns={this.columns}
                defaultSorted={this.defaultSorted}
                pagination={paginationFactory()}
                rowStyle={this.rowStyle}
            />
        );
    }
}

export default CurrencyTable;