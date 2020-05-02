import Axios from 'axios';

const UserService = {
  fetch: async params => Axios.post(`/admin/user/loaddata`, params),
  fetchById: async id => Axios.get(`/admin/user/getbyid/${id}`),
  create: async params => Axios.post(`/admin/user/create`, params),
  edit: async user => Axios.post(`/admin/user/edit`, user),
  delete: async id => Axios.delete(`/admin/user/delete/${id}`)
};

export default UserService;
