import Axios from 'axios';



export default {

    fetch: async(params) => Axios.post(
        `/admin/gate/loaddata`, params
    ),
};