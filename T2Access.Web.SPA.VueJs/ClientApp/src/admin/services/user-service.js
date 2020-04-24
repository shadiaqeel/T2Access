import Axios from 'axios';


const UserService = {
    fetch: async(to, from) => Axios.get(
        `/admin/user/get?start=${from}&length=${to}`
    ),
    delete: async(id) => Axios.delete(
        `/admin/user/delete/${id}`
    )

};


export default UserService;