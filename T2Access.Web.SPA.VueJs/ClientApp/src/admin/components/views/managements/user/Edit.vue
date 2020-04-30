<template >
  <div>
    <h4 style="display: inline;">Edit User</h4>
    <div style="margin: 30px 40px;  " v-if="editUser">
      <el-form label-position="left" label-width="100px" :model="editUser">
        <el-form-item label="User Name" style="width:100%;" :error="modelstate['UserName']">
          <el-input :disabled="true" v-model="editUser.userName"></el-input>
        </el-form-item>
        <div class="row">
          <el-form-item label="First Name" class="col-md-6" :error="modelstate['FirstName']">
            <el-input v-model="editUser.firstName"></el-input>
          </el-form-item>
          <el-form-item label="Last Name" class="col-md-6" :error="modelstate['LastName']">
            <el-input v-model="editUser.lastName"></el-input>
          </el-form-item>
        </div>
        <el-form-item label="Status">
          <el-select v-model="editUser.status" value-key="editUser.status" placeholder="Status">
            <el-option
              v-for="(status, index) in userStatus"
              :key="index"
              :label="status.label"
              :value="index"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-divider>
          <i class="el-icon-star-on"></i>
        </el-divider>

        <el-card :body-style="{padding:'0px'}">
          <div slot="header" class="clearfix">
            <span>Gate Table</span>
          </div>

          <Datatable
            ref="Table"
            :data="gateList"
            rowKey="id"
            :height="250"
            style="width: 100%"
            :loader="loader"
            :infiniteLoading="true"
            @infinite-handler="infiniteHandler"
            spinner="waveDots"
            highlight-current-row
            @row-click="handleRowClick"
            @selected-fields="handleSelectionChange"
          >
            <el-table-column
              type="selection"
              align="center"
              :reserve-selection="true"
              width="55"
              prop="checked"
            ></el-table-column>

            <el-table-column
              min-width="100"
              align="center"
              label="Arabic Name"
              property="nameAr"
              sortable
            ></el-table-column>

            <el-table-column
              min-width="100"
              align="center"
              label="Engilsh Name"
              property="nameEn"
              sortable
            ></el-table-column>
          </Datatable>
        </el-card>

        <div class="mt centered">
          <el-form-item>
            <el-button type="primary" @click="submitForm('editUser')">Edit</el-button>
            <el-button @click="$router.push({name:'user'})">Exit</el-button>
          </el-form-item>
        </div>
      </el-form>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
// import { Notification } from "admin/utils/helper/notification";
import { userStatus } from "admin/types/status";
import Datatable from "admin/components/elements/Datatable";
import gateSerivce from "admin/services/gate-service";
import userSerivce from "admin/services/user-service";

export default {
  name: "EditUser",
  props: ["userId"],
  components: {
    Datatable
  },
  data() {
    return {
      userStatus: userStatus,
      modelstate: {},
      AddedGateList: [],
      RemovedGateList: [],
      gateList: [],
      page: 1,
      loader: false
    };
  },

  computed: {
    ...mapGetters("user", ["editUser"])
  },
  mounted() {
    if (!this.user) {
      this.fetchById();
    }
  },
  methods: {
    fetchById() {
      return this.$store
        .dispatch("user/fetchById", this.$route.params.userId)
        .catch(e => {
          this.$router.push({ name: "user" });
        });
    },
    submitForm(formName) {
      this.modelstate = {};

      this.$refs[formName].validate(valid => {
        if (valid) {
          this.editUser.addedGateList = this.AddedGateList;
          this.editUser.removedGateList = this.RemovedGateList.map(
            gate => gate.id
          ).toString();
          userSerivce
            .create(this.editUser)
            .then(res => {
              if (res.status == 200) {
                this.$notify({
                  group: "main",
                  type: "success",
                  text: res.data
                });
                this.$router.push({ name: "user" });
              }
            })
            .catch(error => {
              if (error.response.status == 400) {
                this.modelstate = JSON.parse(
                  JSON.stringify(error.response.data)
                );
              }
            });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    infiniteHandler($state) {
      var start = (this.page - 1) * 10;
      gateSerivce
        .fetchByUserId(this.$route.params.userId, {
          start: start,
          length: 10
        })
        .then(res => {
          if (res.status == 200 && res.data.list.length) {
            this.page += 1;
            this.gateList = this.gateList.concat(res.data.list);
            const rowsToToggle = res.data.list.filter(row => row.checked);
            this.toggleSelection(rowsToToggle);

            $state.loaded();
          } else if (!this.gateList.length) {
            $state.reset();
            setTimeout(() => {
              $state.complete();
            }, 1);
          } else {
            $state.loaded();
            $state.complete();
          }
        })
        .catch(e => {
          console.log(e);
          $state.error();
        });
    },
    handleSelectionChange(selectedList, item) {
      this.selectedList = selectedList;
      console.log(selectedList);
      console.log(item);
    },
    handleRowClick(row, column, event) {
      this.$refs["Table"].$refs["dataTable"].toggleRowSelection(row);
      console.log(this.$refs["Table"].$refs["dataTable"].selection);
    },
    toggleSelection(rows) {
      if (rows) {
        rows.forEach(row => {
          this.$refs["Table"].$refs["dataTable"].toggleRowSelection(row);
        });
      }
    }
  }
};
</script>

<style lang="scss" scoped>
.el-form--inline .el-form-item__content {
  width: 300px;
}
</style>
