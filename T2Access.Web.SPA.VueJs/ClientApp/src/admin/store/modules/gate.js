import {
    SET_GATES,
    SET_GATE,
    DELETE_GATE,
    SET_EDITGATE,
    EDIT_GATE,
    SET_CURRENT_PAGE,
    SET_PAGE_SIZE,
    SET_TOTAL_IN_SERVER
} from '../mutation-types';

import gateService from '../../services/gate-service';

const INITIAL_STATE = {
    gates: [],
    editGate: null,
    tableOptions: {
        currentPage: 1,
        totalInServer: 0,
        pageSize: 10
    }
};

const state = {...INITIAL_STATE };

const getters = {
    gates: state => state.gates,

    editGate: state => state.editGate,

    hasgates: state => state.gates.length !== 0,

    tableOptions: state => state.tableOptions
};

const mutations = {
    [SET_GATES]: (state, gates) => {
        state.gates = gates;
    },
    [SET_GATE]: (state, gate) => {
        state.gates.push(gate);
        state.tableOptions.totalInServer += 1;
    },
    [SET_EDITGATE]: (state, gate) => {
        state.editGate = gate;
    },
    [EDIT_GATE]: (state, gate) => {
        state.gates[state.gates.findIndex(item => item.id == gate.id)] = gate;
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
    [DELETE_GATE]: (state, gateId) => {
        // state.gates.splice(state.gates.map(item => item.id).indexOf(gateId), 1);
        state.gates.splice(state.gates.findIndex(item => item.id == gateId), 1);
    }
};

const actions = {
    fetchPage: async({ commit }, page) => {
        commit(SET_CURRENT_PAGE, page);
        await gateService
            .fetch({
                start:
                    (state.tableOptions.currentPage - 1) * state.tableOptions.pageSize,
                length: state.tableOptions.pageSize
            })
            .then(res => {
                if (res.status == 200) {
                    commit(SET_GATES, res.data.list);
                    commit(SET_TOTAL_IN_SERVER, res.data.recordsTotal);
                }
            });
    },
    delete: async({ commit }, gateID) => {
        const response = await gateService.delete(gateID);
        if (response.status == 200) {
            commit(DELETE_GATE, gateID);
            return Promise.resolve(response.data);
        } else
            return Promise.reject(response.data);


    },
    edit: async({ commit }, gate) => {
        const response = await gateService.edit(gate);
        if (response.status == 200) {
            commit(EDIT_GATE, gate);
            return Promise.resolve(response.data);
        } else
            return Promise.reject(response.data)


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