import React, {Component} from 'react';
import Chart from 'react-apexcharts';
import {getAllLatestCurrencyData, getLatestCurrencyData} from "../services/currencyService";

const UPDATE_INTERVAL = 10000; // in milliseconds

class Charts extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: [{data: []}],
            selectedCurrency: 'AUDJPY',
            selectedCurrencyValue: 0,
            options: {
                chart: {
                    type: 'candlestick',
                    height: 350
                },
                title: {
                    text: 'CandleStick Chart',
                    align: 'left'
                },
                xaxis: {
                    type: 'datetime'
                },
                yaxis: {
                    tooltip: {
                        enabled: true
                    }
                }
            }
        };
        
        this.updateCurrency = this.updateCurrency.bind(this);
        this.updateChart = this.updateChart.bind(this);
        this.updateWholeChart = this.updateWholeChart.bind(this);
    }
    
    componentDidMount() {
        this.updateWholeChart();
        
        this.interval = setInterval(this.updateChart, UPDATE_INTERVAL);
    }
    
    componentWillUnmount() {
        clearInterval(this.interval);
    }


    updateCurrency(e) {
        console.log('updateCurrency')
        let newCurrency = e.target.firstChild.data;
        this.setState({selectedCurrency: newCurrency}, () => {
            this.updateWholeChart();
        });
    }

    updateChart() {
        console.log('updateChart')
        getLatestCurrencyData(this.props.token, this.state.selectedCurrency)
            .then(result => {
                this.setState({selectedCurrencyValue: Math.round(result.price * 100) / 100})
                let newData = {
                    x: new Date(result.startDate),
                    y: [
                        Math.round(result.open * 100) / 100,
                        Math.round(result.high * 100) / 100,
                        Math.round(result.low * 100) / 100,
                        Math.round(result.close * 100) / 100
                    ]
                };
                
                this.setState((prevState, wtf) => ({
                    data: [{ data: prevState.data[0].data.concat(newData) }]
                }))
            })
    }

    updateWholeChart() {
        console.log('updateWholeChart')
        getAllLatestCurrencyData(this.props.token, this.state.selectedCurrency)
            .then(result => result.map(c => {
                this.setState({selectedCurrencyValue: Math.round(result.at(-1).price * 100) / 100})
                return {
                    x: new Date(c.startDate),
                    y: [
                        Math.round(c.open * 100) / 100,
                        Math.round(c.high * 100) / 100,
                        Math.round(c.low * 100) / 100,
                        Math.round(c.close * 100) / 100
                    ]
                }
            }))
            .then(data => {
                this.setState({data: [{data: data}]})
            })
    }

    render() {
        return (

            <div id="chart">

                <div className="btn-group">
                    <button type="button" className="btn btn-secondary disabled">{this.state.selectedCurrencyValue}</button>
                    <button type="button" className="btn btn-secondary">{this.state.selectedCurrency}</button>
                    <button type="button" className="btn btn-secondary dropdown-toggle dropdown-toggle-split"
                            data-bs-toggle="dropdown" aria-expanded="false">
                        <span className="visually-hidden">Toggle Dropdown</span>
                    </button>
                    <ul className="dropdown-menu">
                        <button className='dropdown-item' onClick={this.updateCurrency}>AUDJPY</button>
                        <button className='dropdown-item' onClick={this.updateCurrency}>AUDUSD</button>
                        <button className='dropdown-item' onClick={this.updateCurrency}>EURGBP</button>
                        <button className='dropdown-item' onClick={this.updateCurrency}>EURJPY</button>
                        <button className='dropdown-item' onClick={this.updateCurrency}>EURUSD</button>
                        <button className='dropdown-item' onClick={this.updateCurrency}>GBPJPY</button>
                        <button className='dropdown-item' onClick={this.updateCurrency}>GBPUSD</button>
                        <button className='dropdown-item' onClick={this.updateCurrency}>USDCAD</button>
                        <button className='dropdown-item' onClick={this.updateCurrency}>USDJPY</button>
                    </ul>
                </div>

                <Chart options={this.state.options} series={this.state.data} type="candlestick" height={350}/>

            </div>

        )
    }
}

export default Charts;
