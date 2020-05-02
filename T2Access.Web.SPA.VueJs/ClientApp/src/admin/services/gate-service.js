import Axios from 'axios';

export default {
  fetch: async params => Axios.post(`/admin/gate/loaddata`, params),
  fetchByUserId: async (userId, params) =>
    Axios.post(`/admin/gate/GetCheckedByUserId/${userId}`, params),

  create: async params => Axios.post(`/admin/gate/create`, params),
  edit: async gate => Axios.post(`/admin/gate/edit`, gate),
  delete: async id => Axios.delete(`/admin/gate/delete/${id}`)
};
