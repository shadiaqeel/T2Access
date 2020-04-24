import Axios from 'axios';



export default {

	fetch: async (to, from) => Axios.get(
		`/gate/get?from=${from}&to=${to}`
	)
};
