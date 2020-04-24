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
        <!-- <select
            asp-items="Html.GetEnumSelectList<T2Access.Models.UserStatus>()"
            class="dropdown-toggle form-control-sm"
            id="txtFilterByStatus"
          >
            <option value>Status</option>
        </select>-->

        <button id="btnSearch" class="fa fa-search btn btn-info btn-lg pull-away"></button>
      </div>
    </div>
    <!-- @*DataTable Area*@ -->
    <Datatable
      :data="users"
      :total-in-server="tableOptions.totalInServer"
      :current-page="tableOptions.currentPage"
      :numPerPage="tableOptions.pageSize"
      :loader="false"
      @current-page="loadPage"
      @size-table="size"
    >
      <el-table-column label="User Name" property="userName" sortable></el-table-column>
      <el-table-column label="First Name" property="firstName" sortable></el-table-column>
      <el-table-column label="Last Name" property="lastName" sortable></el-table-column>
      <el-table-column label="Status" property="status" sortable></el-table-column>
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
import { Notification } from "admin/utils/helper/notification";

import Datatable from "admin/components/elements/Datatable";

export default {
  name: "UserManagement",
  components: {
    Datatable
  },
  data() {
    return {
      loader: false,
      filter: {
        username: "",
        firstname: "",
        lastname: ""
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
    handleEdit(row) {
      console.log(row);
    },
    handleDelete(id) {
      this.$store
        .dispatch("deleteUser", id)
        .then(message => {
          Notification.success(this, message);
        })
        .catch(e => {
          Notification.error(this, e);
        });
      console.log(id);
    }
  }
};
</script>

<style lang="sass" scoped>
</style>
