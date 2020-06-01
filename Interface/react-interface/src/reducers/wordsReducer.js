const wordsReducer = (state, { payload }) => {
	const regex = /^[a-zA-Zăîâșț]{1,}-?[a-zA-Zăîâșț]{0,}$/;
	payload.forEach((word) => {
		if (word.Word.trim().length >= 2 && regex.test(word.Word.trim())) {
			state = {
				...state,
				[word.Word.trim()]: {
					count: word.Count,
					suggestions: word.Suggestions.slice(0, 3)
				}
			};
		}
	});
	return state;
};

const updateWords = (dispatch) => (words) => {
	dispatch({ payload: words });
};

export { wordsReducer, updateWords };
