<template>
  <div>
    <h5 style="display: inline;">{{ $t('gate.table') }}</h5>

    <router-link :to="{ name: 'createGate' }" class="btn btn-lg btn-success pull-away">
      <i class="el-icon-plus"></i>
      {{ $t('gate.add') }}
    </router-link>
    <router-view></router-view>

    <!-- @*Divider*@ -->

    <el-divider></el-divider>
    <!-- Search Area -->

    <div class="row mt mb">
      <div class="dataTables_filter col-md-3">
        <el-input
          size="small"
          clearable
          :placeholder="$t('gate.username')"
          v-model="filter.username"
        ></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-input size="small" clearable :placeholder="$t('gate.nameAr')" v-model="filter.nameAr"></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-input size="small" clearable :placeholder="$t('gate.nameEn')" v-model="filter.nameEn"></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-select
          size="small"
          style="width:150px"
          v-model="filter.status"
          value-key="filter.status"
          clearable
          @clear="filter.status = null"
          :placeholder="$t('gate.status')"
        >
          <el-option
            v-for="(status, index) in gateStatus"
            :key="index"
            :label="$t(`gate.gateStatus.${status.label}`)"
            :value="index"
          ></el-option>
        </el-select>

        <el-button
          size="mini "
          class="pull-away"
          icon="el-icon-search"
          style="border-radius:auto"
          round
          @click="handleFilter()"
        ></el-button>
      </div>
    </div>

    <!-- @*Divider*@ -->

    <el-divider>
      <i class="el-icon-star-on"></i>
      <i class="el-icon-star-on"></i>
      <i class="el-icon-star-on"></i>
    </el-divider>
    <!-- @*DataTable Area*@ -->

    <Datatable
      :data="isFiltered ? gates : gatesState"
      :pagination="true"
      :total-in-server="
        isFiltered
          ? tableOptions.totalInServer
          : tableOptionsState.totalInServer
      "
      :current-page="
        isFiltered ? tableOptions.currentPage : tableOptionsState.currentPage
      "
      :numPerPage="
        isFiltered ? tableOptions.pageSize : tableOptionsState.pageSize
      "
      :loader="loader"
      @current-page="loadPage"
      @size-table="size"
      @sort-change="handleSortChange"
    >
      <el-table-column min-width="100" :label="$t('gate.username')" property="userName" sortable></el-table-column>

      <el-table-column min-width="100" :label="$t('gate.nameAr')" property="nameAr" sortable></el-table-column>

      <el-table-column min-width="100" :label="$t('gate.nameEn')" property="nameEn" sortable></el-table-column>

      <el-table-column min-width="100" :label="$t('gate.status')" property="status" sortable>
        <template slot-scope="scope">
          <el-tag :type="gateStatus[scope.row.status].type" disable-transitions>
            {{
            $t(`gate.gateStatus.${gateStatus[scope.row.status].label}`)
            }}
          </el-tag>
        </template>
      </el-table-column>

      <el-table-column min-width="150" :label="$t('actions')" property="actions" align="center">
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
          <el-button
            size="mini"
            class="btn"
            type="warning"
            icon="el-icon-key"
            @click="handleResetPassword(scope.row)"
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
        status: null,
        sortOrder: null
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
    size(sizeTable) {
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
        this.$t("confirmDelete", [this.$t("gate.gate")]),
        this.$t("delete"),
        {
          confirmButtonText: this.$t("ok"),
          cancelButtonText: this.$t("cancel"),
          type: "warning",
          center: true,
          showClose: false
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
    handleResetPassword(row) {
      console.group("handle Reset Password");
      console.log(row);
      this.$router.push({
        name: "ResetPasswordGate",
        params: { gate: { id: row.id, userName: row.userName } }
      });

      console.groupEnd("handle Reset Password");
    },
    handleSortChange({ prop, order }) {
      console.groupCollapsed("handle Sort Change");
      console.log({ prop, order });
      //if(sort.order =="descending" ) //DESC

      if (order == "descending") this.filter.sortOrder = `${prop} DESC`;
      else if (order) this.filter.sortOrder = prop;
      else this.filter.sortOrder = null;

      this.handleFilter();
      console.log(this.filter.sortOrder);

      console.groupEnd("handle Sort Change");
    },
    handleFilter() {
      if (
        this.filter.username ||
        this.filter.nameAr ||
        this.filter.nameEn ||
        this.filter.sortOrder ||
        this.filter.status != null
      ) {
        this.isFiltered = true;
        this.tableOptions.currentPage = 1;

        var start =
          (this.tableOptions.currentPage - 1) * this.tableOptions.pageSize;
        var length = start + this.tableOptions.pageSize;
        gateService
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

<style lang="scss" scoped></style>
