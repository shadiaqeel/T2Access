<template>
  <div>
    <h4 style="display: inline;">Gate Table</h4>

    <router-link
      :to="{ name: 'createGate' }"
      class="fa fa-plus btn btn-lg btn-success pull-away"
    >Add Gate</router-link>
    <router-view></router-view>

    <!-- Search Area -->

    <div class="row mt mb">
      <div class="dataTables_filter col-md-3">
        <el-input size="small" clearable placeholder="User Name" v-model="filter.username"></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-input size="small" clearable placeholder="Arabic Name" v-model="filter.nameAr"></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-input size="small" clearable placeholder="Engilsh Name" v-model="filter.nameEn"></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-select
          size="small"
          style="width:150px"
          v-model="filter.status"
          value-key="filter.status"
          clearable
          @clear="filter.status = null"
          placeholder="Status"
        >
          <el-option
            v-for="(status, index) in gateStatus"
            :key="index"
            :label="status.label"
            :value="index"
          ></el-option>
        </el-select>

        <el-button
          size="mini"
          class="btn pull-away"
          type="info"
          plain
          icon="el-icon-search"
          @click="handleFilter()"
        ></el-button>
      </div>
    </div>

    <!-- @*DataTable Area*@ -->

    <Datatable
      :data="isFiltered?gates:gatesState"
      :pagination="true"
      :total-in-server="isFiltered? tableOptions.totalInServer : tableOptionsState.totalInServer "
      :current-page="isFiltered? tableOptions.currentPage : tableOptionsState.currentPage"
      :numPerPage="isFiltered? tableOptions.pageSize : tableOptionsState.pageSize"
      :loader="loader"
      @current-page="loadPage"
      @size-table="size"
    >
      <el-table-column min-width="100" label="User Name" property="userName" sortable></el-table-column>

      <el-table-column min-width="100" label="Arabic Name" property="nameAr" sortable></el-table-column>

      <el-table-column min-width="100" label="Engilsh Name" property="nameEn" sortable></el-table-column>

      <el-table-column min-width="100" label="Status" property="status" sortable>
        <template slot-scope="scope">
          <el-tag
            :type="gateStatus[scope.row.status].type"
            disable-transitions
          >{{ gateStatus[scope.row.status].label }}</el-tag>
        </template>
      </el-table-column>

      <el-table-column label="Actions" property="actions">
        <template slot-scope="scope">
          <el-button
            size="mini"
            type="primary"
            class="btn"
            icon="el-icon-edit"
            @click="handleEdit(scope.row)"
          ></el-button>
          <el-button
            size="mini"
            class="btn"
            type="danger"
            icon="el-icon-delete"
            @click="handleDelete(scope.row.id)"
          ></el-button>
        </template>
      </el-table-column>
    </Datatable>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import { SET_EDITGATE } from "admin/store/mutation-types";
import { gateStatus } from "admin/types/status";
import gateService from "admin/services/gate-service";

import Datatable from "admin/components/elements/Datatable";

export default {
  name: "gateManagement",
  components: {
    Datatable
  },
  data() {
    return {
      gates: [],
      tableOptions: {
        currentPage: 1,
        totalInServer: 0,
        pageSize: 10
      },
      gateStatus: gateStatus,
      loader: false,
      isFiltered: false,
      filter: {
        username: null,
        nameAr: null,
        nameEn: null,
        status: null
      }
    };
  },
  computed: {
    ...mapGetters("gate", {
      gatesState: "gates",
      hasgates: "hasgates",
      tableOptionsState: "tableOptions"
    })
  },
  created() {
    // if (!this.hasgates)
    this.loadPage(this.tableOptionsState.currentPage);
  },
  methods: {
    async loadPage(page) {
      this.loader = true;

      if (this.isFiltered) {
        this.tableOptions.currentPage = page;
        var start =
          (this.tableOptions.currentPage - 1) * this.tableOptions.pageSize;
        var length = start + this.tableOptions.pageSize;
        await gateService
          .fetch({
            start: start,
            length: length,
            filter: this.filter
          })
          .then(res => {
            console.log(res);
            console.log("isFiltered/then");
            if (res.status == 200) {
              this.gates = res.data.list;
              this.tableOptions.totalInServer = res.data.recordsTotal;
            }
          })
          .finally(() => {
            this.loader = false;
          });
      } else {
        this.$store
          .dispatch("gate/fetchPage", page)
          .catch(() => {
            this.$notify({
              group: "main",
              type: "error",
              text: "Error fetching gates."
            });
          })
          .finally(() => {
            this.loader = false;
          });
      }
    },
    async size(sizeTable) {
      if (this.isFiltered) {
        this.tableOptions.pageSize = sizeTable;
        this.loadPage(this.tableOptions.currentPage);
      } else
        this.$store
          .dispatch("gate/changePageSize", sizeTable)
          .finally(() => this.loadPage(this.tableOptionsState.currentPage));
    },
    handleEdit(row) {
      this.$store.commit(`gate/${SET_EDITGATE}`, { ...row });
      this.$router.push({ name: "EditGate", params: { gateId: row.id } });
    },
    handleDelete(id) {
      this.$confirm(
        "This will permanently delete the file. Continue?",
        "Warning",
        {
          confirmButtonText: "OK",
          cancelButtonText: "Cancel",
          type: "warning"
        }
      ).then(() => {
        this.$store
          .dispatch("gate/delete", id)
          .then(message => {
            this.$notify({ group: "main", type: "success", text: message });
          })
          .catch(message => {
            this.$notify({ group: "main", type: "error", text: message });
          });
      });
    },
    async handleFilter() {
      if (
        this.filter.username ||
        this.filter.nameAr ||
        this.filter.nameEn ||
        this.filter.status != null
      ) {
        this.isFiltered = true;
        this.tableOptions.currentPage = 1;

        var start =
          (this.tableOptions.currentPage - 1) * this.tableOptions.pageSize;
        var length = start + this.tableOptions.pageSize;
        await gateService
          .fetch({
            start: start,
            length: length,
            filter: this.filter
          })
          .then(res => {
            if (res.status == 200) {
              this.gates = res.data.list;
              this.tableOptions.totalInServer = res.data.recordsTotal;
            }
          });
      } else {
        this.isFiltered = false;
      }
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
