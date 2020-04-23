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
        <input
          id="txtFilterByUserName"
          type="search"
          class="form-control form-control-sm"
          placeholder="UserName"
          aria-controls="DTable"
        />
      </div>
      <div class="dataTables_filter col-md-3">
        <input
          id="txtFilterByFirstName"
          type="search"
          class="form-control form-control-sm"
          placeholder="FirstName"
          aria-controls="DTable"
        />
      </div>
      <div class="dataTables_filter col-md-3">
        <input
          id="txtFilterByLastName"
          type="search"
          class="form-control form-control-sm"
          placeholder="LastName"
          aria-controls="DTable"
        />
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
    <Datatable :data="users" :total-in-server="1" :columns="columns" :loader="false"></Datatable>
  </div>
</template>

<script>
import Datatable from "../../../elements/Datatable";

export default {
  name: "UserManagement",
  components: {
    Datatable
  },
  data() {
    return {
      columns: [
        {
          width: "33%%",
          label: "UserName",
          name: "username",
          type: "String",
          icon: "fa-user"
        },
        {
          width: "33%%",
          label: "FirstName",
          name: "firstname",
          type: "String",
          icon: "fa-user"
        },
        {
          width: "33%%",
          label: "LastName",
          name: "lastname",
          type: "String",
          icon: "fa-user"
        },
        {
          width: "33%%",
          label: "Status",
          name: "status",
          type: "String",
          icon: "fa-edit"
        },
        {
          width: "33%%",
          label: "Actions",
          name: "actions",
          type: "String",
          icon: "fa-flash"
        }
      ],
      users: [
        { username: "shadi Akil", firstname: "shadi", lastname: "akil" , status : 'admin'},
        { username: "shadi Akil", firstname: "shadi", lastname: "akil" , status : 'admin' },
        { username: "shadi Akil", firstname: "shadi", lastname: "akil" , status : 'block' },
        { username: "shadi Akil", firstname: "shadi", lastname: "akil"  , status : 'admin'},
        { username: "shadi Akil", firstname: "shadi", lastname: "akil" , status : 'admin' },
        { username: "shadi Akil", firstname: "shadi", lastname: "akil" , status : 'admin' },
        { username: "shadi Akil", firstname: "shadi", lastname: "akil" , status : 'admin'},
        { username: "shadi Akil", firstname: "shadi", lastname: "akil" , status : 'admin' }
      ]
    };
  }
};
</script>

<style lang="sass" scoped>
</style>
