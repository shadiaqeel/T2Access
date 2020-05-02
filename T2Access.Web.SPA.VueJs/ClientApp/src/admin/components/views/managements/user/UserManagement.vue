<template>
  <div>
    <h5 style="display: inline;">{{ $t('user.table') }}</h5>
    <router-link
      :to="{ name: 'createUser' }"
      class="fa fa-plus btn btn-lg btn-success pull-away"
      >{{ $t('user.new') }}</router-link
    >

    <!-- @*Divider*@ -->
    <el-divider></el-divider>
    <!-- Search Area -->

    <div class="row mt mb">
      <div class="dataTables_filter col-md-3">
        <el-input
          size="small"
          clearable
          :placeholder="$t('user.username')"
          v-model="filter.username"
        ></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-input
          size="small"
          clearable
          :placeholder="$t('user.firstname')"
          v-model="filter.firstname"
        ></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-input
          size="small"
          clearable
          :placeholder="$t('user.lastname')"
          v-model="filter.lastname"
        ></el-input>
      </div>

      <div class="dataTables_filter col-md-3">
        <el-select
          size="small"
          style="width:150px"
          v-model="filter.status"
          value-key="filter.status"
          clearable
          @clear="filter.status = null"
          :placeholder="$t('user.status')"
        >
          <el-option
            v-for="(status, index) in userStatus"
            :key="index"
            :label="$t(`user.userStatus.${status.label}`)"
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

    <!-- @*Divider*@ -->

    <el-divider>
      <i class="el-icon-star-on"></i>
      <i class="el-icon-star-on"></i>
      <i class="el-icon-star-on"></i>
    </el-divider>

    <!-- @*DataTable Area*@ -->

    <Datatable
      :data="isFiltered ? users : usersState"
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
      <el-table-column
        min-width="100"
        :label="$t('user.username')"
        property="userName"
        sortable
      ></el-table-column>

      <el-table-column
        min-width="100"
        :label="$t('user.firstname')"
        property="firstName"
        sortable
      ></el-table-column>

      <el-table-column
        min-width="100"
        :label="$t('user.lastname')"
        property="lastName"
        sortable
      ></el-table-column>

      <el-table-column
        min-width="100"
        :label="$t('user.status')"
        property="status"
        sortable
      >
        <template slot-scope="scope">
          <el-tag
            :type="userStatus[scope.row.status].type"
            disable-transitions
            >{{
              $t(`user.userStatus.${userStatus[scope.row.status].label}`)
            }}</el-tag
          >
        </template>
      </el-table-column>

      <el-table-column :label="$t('actions')" property="actions" align="center">
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
import { mapGetters } from 'vuex';
// import { Notification } from "admin/utils/helper/notification";
import { userStatus } from 'admin/types/status';
import UserService from 'admin/services/user-service';

import Datatable from 'admin/components/elements/Datatable';

export default {
  name: 'UserManagement',
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
        status: null,
        sortOrder: null
      }
    };
  },
  computed: {
    ...mapGetters('user', {
      usersState: 'users',
      hasUsers: 'hasUsers',
      tableOptionsState: 'tableOptions'
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
        this.tableOptions.currentPage = page;
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
          .dispatch('user/fetchPage', page)

          .catch(() => {
            this.$notify({
              group: 'main',
              type: 'error',
              text: 'Error fetching Users.'
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
          .dispatch('user/changePageSize', sizeTable)
          .finally(() => this.loadPage(this.tableOptionsState.currentPage));
    },
    handleEdit(id) {
      this.$router.push({ name: 'EditUser', params: { userId: id } });
    },
    handleDelete(id) {
      this.$confirm(
        'This will permanently delete the file. Continue?',
        'Warning',
        {
          confirmButtonText: 'OK',
          cancelButtonText: 'Cancel',
          type: 'warning'
        }
      ).then(() => {
        this.$store
          .dispatch('user/delete', id)
          .then(message => {
            this.$notify({
              group: 'main',
              type: 'success',
              text: message
            });
          })
          .catch(message => {
            this.$notify({
              group: 'main',
              type: 'error',
              text: message
            });
          });
      });
    },
    handleSortChange({ prop, order }) {
      console.groupCollapsed('handle Sort Change');
      console.log({ prop, order });
      //if(sort.order =="descending" ) //DESC

      if (order) this.filter.sortOrder = prop;
      else if (order == 'descending') this.filter.sortOrder = `${prop} DESC`;
      else this.filter.sortOrder = null;

      this.handleFilter();
      console.log(this.filter.sortOrder);

      console.groupEnd('handle Sort Change');
    },
    handleFilter() {
      if (
        this.filter.username ||
        this.filter.firstname ||
        this.filter.lastname ||
        this.filter.sortOrder ||
        this.filter.status != null
      ) {
        this.isFiltered = true;

        this.tableOptions.currentPage = 1;

        var start =
          (this.tableOptions.currentPage - 1) * this.tableOptions.pageSize;
        var length = start + this.tableOptions.pageSize;
        UserService.fetch({
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
        console.log('filter2');

        this.isFiltered = false;
      }
    }
  }
};
</script>

<style lang="scss" scoped></style>
