import React, { createContext, useReducer, useEffect } from 'react';
import PropTypes from 'prop-types';

import useWebSocket from 'react-use-websocket';
import { wordsReducer, updateWords } from '../reducers/wordsReducer';

const Context = createContext();

const Provider = ({ children }) => {
	const { lastMessage } = useWebSocket('ws://127.0.0.1:8081/chanel');

	const [ wordsStats, dispatch ] = useReducer(wordsReducer, {});

	useEffect(
		() => {
			if (lastMessage) {
				const result = JSON.parse(lastMessage.data).Result;
				updateWords(dispatch)(result);
			}
		},
		[ lastMessage ]
	);

	return (
		<Context.Provider
			value={{
				wordsStats
			}}
		>
			{children}
		</Context.Provider>
	);
};

Provider.propTypes = {
	children: PropTypes.node
};

export { Provider, Context };
