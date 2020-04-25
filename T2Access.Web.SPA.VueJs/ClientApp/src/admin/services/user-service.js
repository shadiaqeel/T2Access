import Axios from 'axios';


const UserService = {
    fetch: async(params) => Axios.post(
        `/admin/user/loaddata`, params
    ),
    delete: async(id) => Axios.delete(
        `/admin/user/delete/${id}`
    )

};


export default UserService;