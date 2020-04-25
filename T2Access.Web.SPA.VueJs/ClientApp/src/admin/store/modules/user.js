import {
    SET_USERS,
    // CREATE_USER,
    DELETE_USER,
    // EDIT_USER,
    SET_CURRENT_PAGE,
    SET_PAGE_SIZE,
    SET_TOTAL_IN_SERVER
} from '../mutation-types';

import UserService from '../../services/user-service';



const from = () => (state.tableOptions.currentPage - 1) * state.tableOptions.pageSize;
const to = () => from() + state.tableOptions.pageSize;

const INITIAL_STATE = {
    users: [],
    user: null,
    tableOptions: {
        currentPage: 1,
        totalInServer: 0,
        pageSize: 10
    }
};


const state = Object.assign({}, INITIAL_STATE);


const getters = {
    users: state => state.users,

    getUser: (state) => (Id) => { return state.users.find(user => user.id === Id) },

    hasUsers: state => state.users.length !== 0,

    tableOptions: state => state.tableOptions
};


const mutations = {
    [SET_USERS]: (state, users) => {
        state.users = users;
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
        state.users.splice(state.users.map(item => item.id).indexOf(userId), 1)
    }

};

const actions = {
    fetchUsers: async({ commit }, page) => {
        commit(SET_CURRENT_PAGE, page);

        const response = await UserService.fetch({start:from(), length : from());
        if (response.status == 200) {
            commit(SET_USERS, response.data.users);
            commit(SET_TOTAL_IN_SERVER, response.data.recordsTotal);
        }
    },
    deleteUser: async({ commit }, userID) => {

        const response = await UserService.delete(userID);
        return new Promise((resolve, reject) => {
            if (response.data.success) {
                commit(DELETE_USER, userID);
                resolve(response.data.message);
            } else
                reject(response.data.message);
        });
    },

    changePageSize: ({ commit }, pageSize) => {
        commit(SET_PAGE_SIZE, pageSize);
    }

};



export default {
    state,
    getters,
    mutations,
    actions
};