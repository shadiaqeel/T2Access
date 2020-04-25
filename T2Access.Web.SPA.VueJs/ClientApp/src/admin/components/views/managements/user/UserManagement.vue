<template>
  <div>
    <h4 style="display: inline;">User Table</h4>

    <router-link
      :to="{ name: 'createUser'}"
      class="fa fa-plus btn btn-lg btn-success pull-away"
    >New User</router-link>

    <!-- Search Area -->
    <div class="row mt mb">
      <div class="dataTables_filter col-md-3">
        <el-input size="small" placeholder="User Name" v-model="filter.username"></el-input>
      </div>
      <div class="dataTables_filter col-md-3">
        <el-input size="small" placeholder="First Name" v-model="filter.firstname"></el-input>
      </div>
      <div class="dataTables_filter col-md-3">
        <el-input size="small" placeholder="Last Name" v-model="filter.lastname"></el-input>
      </div>
      <div class="dataTables_filter col-md-3">
        <el-select size="small" style="width:150px" 
         v-model="filter.status" value-key="filter.status" clearable  placeholder="Status">
          
                    <el-option
            v-for="(status, index) in userStatus"
            :key="index"
            :label="status.label"
            :value="index"
          ></el-option>
        </el-select>

        <button id="btnSearch" class="fa fa-search btn btn-info btn-lg pull-away"></button>
      </div>
    </div>
    <!-- @*DataTable Area*@ -->
    <Datatable
      :data="users"
      :total-in-server="tableOptions.totalInServer"
      :current-page="tableOptions.currentPage"
      :numPerPage="tableOptions.pageSize"
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
          >{{userStatus[scope.row.status].label}}</el-tag>
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
import UserService from 'admin/services/user-service';


import Datatable from "admin/components/elements/Datatable";

export default {
  name: "UserManagement",
  components: {
    Datatable
  },
  data() {
    return {
      userStatus : userStatus,
      loader: false,
      isFiltered : false,
      filter: {
        username: '',
        firstname: '',
        lastname: '',
        status : ''
      }
    };
  },
  computed: {
    ...mapGetters(["users", "hasUsers", "tableOptions"])
  },
  created() {
    if (!this.hasUsers) this.loadPage(this.tableOptions.currentPage);
  },
  methods: {
    async loadPage(page) {
      this.loader = true;

      this.$store
        .dispatch("fetchUsers", page)
        .then(() => {
          Notification.success(this, "Users fetched successfully.");
        })
        .catch(() => {
          Notification.error(this, "Error fetching Users.");
        })
        .finally(() => {
          this.loader = false;
        });
    },
    async size(sizeTable) {
      this.$store
        .dispatch("changePageSize", sizeTable)
        .finally(() => this.loadPage(this.tableOptions.currentPage));
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
    }
  }
};
</script>

<style lang="scss" scoped>
</style>
