import React, { useContext, useEffect, useState } from 'react';

import { Context as AppContext } from '../context/index';
import Highcharts from 'highcharts';
import HighchartsReact from 'highcharts-react-official';
import HighchartsMore from 'highcharts-more';
import DarkUnica from 'highcharts/themes/high-contrast-dark';

HighchartsMore(Highcharts);
DarkUnica(Highcharts);

const Graphic = () => {
	const context = useContext(AppContext);

	const { wordsStats } = context;
	const [ words, setWords ] = useState([]);
	const [ counts, setCounts ] = useState([]);

	const chartOptions = {
		chart: {
			type: 'column'
		},
		title: {
			text: 'Most misspelled words'
		},
		xAxis: {
			categories: words
		},
		yAxis: {
			min: 0,
			title: {
				text: 'Count'
			}
		},
		tooltip: {
			formatter: function() {
				let str1 = `<div>The word <b>${this.x}</b> was counted <b>${this
					.y}</b> times. <br />Suggestions: <br/><ul>`;
				wordsStats[this.x].suggestions.forEach(
					(suggestion) => (str1 = str1 + `<li> -> ${suggestion}</li> <br />`)
				);
				return str1 + '</ul></div>';
			}
		},
		plotOptions: {
			column: {
				pointPadding: 0.2,
				borderWidth: 0
			}
		},
		series: [
			{
				data: counts
			}
		],
		legend: {
			enabled: false
		}
	};

	useEffect(
		() => {
			setWords([]);
			setCounts([]);
			const words = [],
				counts = [];
			Object.keys(wordsStats).forEach((word) => {
				words.push(word);
				counts.push(wordsStats[word].count);
			});
			setWords(words);
			setCounts(counts);
		},
		[ wordsStats ]
	);

	return (
		<div style={{ marginTop: '7%' }}>
			<HighchartsReact options={chartOptions} highcharts={Highcharts} className={{ minHeight: 600 }} />
		</div>
	);
};

export default Graphic;
