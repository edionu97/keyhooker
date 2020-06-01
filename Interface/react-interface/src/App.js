import React from 'react';
import './App.css';
import { Provider } from './context';

import Graphic from './components/Graphic';

const App = (props) => {
	return (
		<Provider>
			<Graphic />
		</Provider>
	);
};

export default App;
