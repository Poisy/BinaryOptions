import React, {Component} from 'react';
import {getAllLatestCurrencyData, getLatestCurrencyData} from "../services/currencyService";
import Highcharts from 'highcharts/highstock'
import Highchart from "./Highchart";
import CurrencyTable from "./CurrencyTable";


const UPDATE_INTERVAL = 10000; // in milliseconds

class Charts extends Component {

    constructor(props) {
        super(props);

        this.state = {
            data: [],
            chartType: 'candlestick',
            selectedCurrency: 'AUDJPY',
            selectedCurrencyValue: 0
        };
        
        this.updateCurrency = this.updateCurrency.bind(this);
        this.updateChart = this.updateChart.bind(this);
        this.updateWholeChart = this.updateWholeChart.bind(this);
        this.changeChart = this.changeChart.bind(this);
    }

    componentDidMount() {
        this.updateWholeChart();
        this.interval = setInterval(this.updateChart, UPDATE_INTERVAL);
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }
    
    
    changeChart(e) {
        console.log('changeChart')
        let type = e.target.firstChild.data.toLowerCase();
        this.setState({chartType: type})
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
                let newData = [
                    new Date(result.startDate).getTime(),
                    Math.round(result.open * 100) / 100,
                    Math.round(result.high * 100) / 100,
                    Math.round(result.low * 100) / 100,
                    Math.round(result.close * 100) / 100
                ];
                
                if (!this.state.data.includes(newData)) {
                    this.setState((prevState) => ({
                        data: [...prevState.data, newData]
                    }));
                }
            })
    }

    updateWholeChart() {
        console.log('updateWholeChart')
        getAllLatestCurrencyData(this.props.token, this.state.selectedCurrency)
            .then(result => result.map(c => {
                this.setState({selectedCurrencyValue: Math.round(result.at(-1).price * 100) / 100})
                return [
                    new Date(c.startDate).getTime(),
                    Math.round(c.open * 100) / 100,
                    Math.round(c.high * 100) / 100,
                    Math.round(c.low * 100) / 100,
                    Math.round(c.close * 100) / 100
                ];
            }))
            .then(data => {
                this.setState({data: data})
            })
    }

    render() {
        return (

            <div>

                <div className="btn-group m-3">
                    <div className="btn-group">
                        <button type="button" className="btn btn-secondary dropdown-toggle dropdown-toggle-split"
                                data-bs-toggle="dropdown" aria-expanded="false">
                            <span className="visually-hidden">Toggle Dropdown</span>
                        </button>

                        <ul className="dropdown-menu">
                            <button className='dropdown-item' onClick={this.changeChart}>Candlestick</button>
                            <button className='dropdown-item' onClick={this.changeChart}>OHLC</button>
                            <button className='dropdown-item' onClick={this.changeChart}>Table</button>
                        </ul>

                        <button type="button" className="btn btn-secondary">{this.state.chartType.toUpperCase()}</button>
                    </div>
                    
                    <button type="button" className="btn btn-secondary disabled">{this.state.selectedCurrencyValue}</button>
                    
                    <div className="btn-group">
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
                    
                </div>
                
                {
                    this.state.chartType === 'candlestick' || this.state.chartType === 'ohlc'
                        ? <Highchart data={this.state.data} type={this.state.chartType} currency={this.state.selectedCurrency}></Highchart> 
                        : <CurrencyTable data={this.state.data}></CurrencyTable>
                }

            </div>

        )
    }
}

export default Charts;
