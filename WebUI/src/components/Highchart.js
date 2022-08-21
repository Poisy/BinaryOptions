import React, {Component} from 'react';
import Highcharts from "highcharts/highstock";

const options = {
    plotOptions: {
        candlestick: {
            upColor: '#99ffa8'
        }
    },
    accessibility: {
        enabled: false
    },
    time: {
        useUTC: false
    },
    rangeSelector: {
        buttons: [{
            count: 1,
            type: 'minute',
            text: '1M'
        }, {
            count: 5,
            type: 'minute',
            text: '5M'
        }, {
            type: 'all',
            text: 'All'
        }],
        inputEnabled: true,
        selected: 2
    },
    exporting: {
        enabled: false
    },
    series: []
}




class Highchart extends Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        this.chart = Highcharts.stockChart('chart', options);
        
        this.updateWholeChart()
        
        this.updateChartType = this.updateChartType.bind(this);
    }
    
    componentDidUpdate(prevProps, prevState, snapshot) {
        if (prevProps.type !== this.props.type) {
            this.updateChartType();
        } else  {
            let previousLast = JSON.stringify(prevProps.data[prevProps.data.length-1]);
            let thisLast = JSON.stringify(this.props.data[this.props.data.length-2]);
            
            if (previousLast !== thisLast) {
                this.updateWholeChart();   
            } else {
                this.updateSingleChart();
            }
        }
    }
    
    updateChartType() {
        console.log('Highchart:updatedChartType')
        if (this.props.type === 'ohlc' || this.props.type === 'candlechart') {
            this.chart.series[0].update({ type: this.props.type }, true);
        }
    }
    
    updateSingleChart() {
        console.log('Highchart:updatedSingleChart')
        this.chart.series[0].addPoint(this.props.data[this.props.data.length-1], true, true);
    }
    
    updateWholeChart() {
        console.log('Highchart:updatedWholeChart')
        if (this.chart.series.length > 0) {
            this.chart.series[0].remove(true, false)
        }

        this.chart.addSeries({
            type: this.props.type,
            data: this.props.data,
            name: this.props.currency
        }, true, true);
    }

    
    
    render() {
        return (
            <div id='chart'></div>
        );
    }
}

export default Highchart;