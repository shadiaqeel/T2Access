import {
  SET_USERS,
  SET_USER,
  // CREATE_USER,
  DELETE_USER,
  // EDIT_USER,
  SET_EDITUSER,
  SET_CURRENT_PAGE,
  SET_PAGE_SIZE,
  SET_TOTAL_IN_SERVER
} from '../mutation-types';

import UserService from '../../services/user-service';

const INITIAL_STATE = {
  users: [],
  editUser: null,
  tableOptions: {
    currentPage: 1,
    totalInServer: 0,
    pageSize: 10
  }
};

const state = { ...INITIAL_STATE };

const getters = {
  users: state => state.users,

  editUser: state => state.editUser,

  hasUsers: state => state.users.length !== 0,

  tableOptions: state => state.tableOptions
};

const mutations = {
  [SET_USERS]: (state, users) => {
    state.users = users;
  },
  [SET_USER]: (state, user) => {
    state.users.push(user);
    state.tableOptions.totalInServer += 1;
  },
  [SET_EDITUSER]: (state, user) => {
    state.editUser = user;
  },
  [SET_TOTAL_IN_SERVER]: (state, totalInServer) => {
    state.tableOptions.totalInServer = totalInServer;
  },

  [SET_PAGE_SIZE]: (state, pageSize) => {
    state.tableOptions.pageSize = pageSize;
  },

  [SET_CURRENT_PAGE]: (state, currentPage) => {
    state.tableOptions.currentPage = currentPage;
  },
  [DELETE_USER]: (state, userId) => {
    state.users.splice(state.users.map(item => item.id).indexOf(userId), 1);
  }
};

const actions = {
  fetchPage: async ({ commit }, page) => {
    commit(SET_CURRENT_PAGE, page);
    await UserService.fetch({
      start: (state.tableOptions.currentPage - 1) * state.tableOptions.pageSize,
      length: state.tableOptions.pageSize
    }).then(res => {
      if (res.status == 200) {
        commit(SET_USERS, res.data.users);
        commit(SET_TOTAL_IN_SERVER, res.data.recordsTotal);
      }
    });
  },
  fetchById: async ({ commit }, userId) => {
    await UserService.fetchById(userId).then(res => {
      console.log(res);
      if (res.status == 200) {
        commit(SET_EDITUSER, res.data);
      }
    });
  },
  delete: async ({ commit }, userID) => {
    const response = await UserService.delete(userID);
    return new Promise((resolve, reject) => {
      if (response.data.success) {
        commit(DELETE_USER, userID);
        resolve(response.data.message);
      } else reject(response.data.message);
    });
  },

  changePageSize: ({ commit }, pageSize) => {
    commit(SET_PAGE_SIZE, pageSize);
  }
};

export default {
  namespaced: true,
  state,
  getters,
  mutations,
  actions
};
