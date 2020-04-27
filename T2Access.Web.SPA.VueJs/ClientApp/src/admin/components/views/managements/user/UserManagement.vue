<template>
  <div>
    <h4 style="display: inline;">User Table</h4>

    <router-link
      :to="{ name: 'createUser' }"
      class="fa fa-plus btn btn-lg btn-success pull-away"
    >New User</router-link>

    <!-- Search Area -->

    <div class="row mt mb">
      <div class="dataTables_filter col-md-3">
        <el-input size="small" clearable placeholder="User Name" v-model="filter.username"></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-input size="small" clearable placeholder="First Name" v-model="filter.firstname"></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-input size="small" clearable placeholder="Last Name" v-model="filter.lastname"></el-input>
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
            v-for="(status, index) in userStatus"
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
      :data="isFiltered?users:usersState"
      :pagination ="true"
      :total-in-server="isFiltered? tableOptions.totalInServer : tableOptionsState.totalInServer "
      :current-page="isFiltered? tableOptions.currentPage : tableOptionsState.currentPage"
      :numPerPage="isFiltered? tableOptions.pageSize : tableOptionsState.pageSize"
      :loader="loader"
      @current-page="loadPage"
      @size-table="size"
    >
      <el-table-column min-width="100" label="User Name" property="userName" sortable></el-table-column>

      <el-table-column min-width="100" label="First Name" property="firstName" sortable></el-table-column>

      <el-table-column min-width="100" label="Last Name" property="lastName" sortable></el-table-column>

      <el-table-column min-width="100" label="Status" property="status" sortable>
        <template slot-scope="scope">
          <el-tag
            :type="userStatus[scope.row.status].type"
            disable-transitions
          >{{ userStatus[scope.row.status].label }}</el-tag>
        </template>
      </el-table-column>

      <el-table-column label="Actions" property="actions">
        <template slot-scope="scope">
          <el-button
            size="mini"
            type="primary"
            class="btn"
            icon="el-icon-edit"
            @click="handleEdit(scope.row.id)"
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
import { Notification } from "admin/utils/helper/notification";
import { userStatus } from "admin/types/status";
import UserService from "admin/services/user-service";

import Datatable from "admin/components/elements/Datatable";

export default {
  name: "UserManagement",
  components: {
    Datatable
  },
  data() {
    return {
      users: [],
      tableOptions: {
        currentPage: 1,
        totalInServer: 0,
        pageSize: 10
      },
      userStatus: userStatus,
      loader: false,
      isFiltered: false,
      filter: {
        username: null,
        firstname: null,
        lastname: null,
        status: null
      }
    };
  },
  computed: {
    ...mapGetters({
      usersState: "users",
      hasUsers: "hasUsers",
      tableOptionsState: "tableOptions"
    })
  },
  created() {
   // if (!this.hasUsers) 
    this.loadPage(this.tableOptionsState.currentPage);
  },
  methods: {
    async loadPage(page) {
      this.loader = true;

      if (this.isFiltered) {
        var start =
          (this.tableOptions.currentPage - 1) * this.tableOptions.pageSize;
        var length = start + this.tableOptions.pageSize;
        await UserService.fetch({
          start: start,
          length: length
        }).then(res => {
          if (res.data.status == 200) {
            this.users = res.data.data.users;
            this.tableOptions.totalInServer = res.data.data.recordsTotal;
          }
        });
      } else {
        this.$store
          .dispatch("fetchUsers", page)

          .catch(() => {
            Notification.error(this, "Error fetching Users.");
          })
          .finally(() => {
            this.loader = false;
          });
      }
    },
    async size(sizeTable) {
      this.$store
        .dispatch("changePageSize", sizeTable)
        .finally(() => this.loadPage(this.tableOptionsState.currentPage));
    },
    handleEdit(id) {
      this.$router.push({ name: "EditUser", params: { userId: id } });
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
          .dispatch("deleteUser", id)
          .then(message => {
            Notification.success(this, message);
          })
          .catch(message => {
            Notification.error(this, message);
          });
      });
    },
    async handleFilter() {
      console.log(this.filter);

      if (
        this.filter.username ||
        this.filter.firstname ||
        this.filter.lastname ||
        this.filter.status !=null
      ) {
        this.isFiltered = true;

        console.log("filter1");

        var start =
          (this.tableOptions.currentPage - 1) * this.tableOptions.pageSize;
        var length = start + this.tableOptions.pageSize;
        await UserService.fetch({
          start: start,
          length: length,
          filter: this.filter
        }).then(res => {
          console.log(res);
          if (res.status == 200) {
            this.users = res.data.users;
            this.tableOptions.totalInServer = res.data.recordsTotal;
          }
        });
      } else {
        console.log("filter2");

        this.isFiltered = false;
      }
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
