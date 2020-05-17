<template>
  <div>
    <el-divider content-position="center">
      <h4 style="display: inline;">{{$t('user.edit')}}</h4>
    </el-divider>
    <div style="margin: 30px 40px;  " v-if="editUser">
      <el-form
        label-position="top"
        label-width="100px"
        :model="editUser"
        ref="editUser"
        :rules="rules"
        status-icon
        hide-required-asterisk
        size="medium"
      >
        <el-form-item
          :label="$t('user.username')"
          style="width:100%;"
          :error="modelstate['UserName']"
        >
          <el-input :disabled="true" v-model="editUser.userName"></el-input>
        </el-form-item>
        <div class="row">
          <el-form-item
            :label="$t('user.firstname')"
            class="col-md-6"
            prop="firstName"
            :error="modelstate['FirstName']"
          >
            <el-input v-model="editUser.firstName"></el-input>
          </el-form-item>
          <el-form-item
            :label="$t('user.lastname')"
            class="col-md-6"
            prop="lastName"
            :error="modelstate['LastName']"
          >
            <el-input v-model="editUser.lastName"></el-input>
          </el-form-item>
        </div>
        <el-form-item :label="$t('user.status')">
          <el-select
            v-model="editUser.status"
            value-key="editUser.status"
            :placeholder="$t('user.username')"
          >
            <el-option
              v-for="(status, index) in userStatus"
              :key="index"
              :label="$t(`user.userStatus.${status.label}`)"
              :value="index"
            ></el-option>
          </el-select>
        </el-form-item>

        <el-divider>
          <i class="el-icon-star-on"></i>
        </el-divider>

        <el-card :body-style="{ padding: '0px' }">
          <div slot="header" class="clearfix">
            <span>Gate Table</span>
          </div>

          <Datatable
            ref="dataTable"
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
            @selection-change="handleSelectionChange"
            @select-all="handleSelectAll"
            @select="handleSelect"
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
              :label="$t('gate.nameAr')"
              property="nameAr"
              sortable
            ></el-table-column>

            <el-table-column
              min-width="100"
              align="center"
              :label="$t('gate.nameEn')"
              property="nameEn"
              sortable
            ></el-table-column>
          </Datatable>
        </el-card>

        <div class="mt centered">
          <el-form-item>
            <el-button
              :loading="isLoading"
              type="primary"
              @click="submitForm('editUser')"
            >{{$t('edit')}}</el-button>
            <el-button @click="$router.push({ name: 'user' })">{{$t('cancel')}}</el-button>
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
      selectedGateList: [],
      addedGateList: [],
      removedGateList: [],
      gateList: [],
      page: 1,
      loader: false,
      isLoading: false,
      rules: {
        firstName: [
          {
            required: true,
            message: this.$t("validate.missInput", {
              input: this.$t("user.firstname").toLowerCase()
            }),
            trigger: "blur"
          },
          {
            min: 3,
            max: 20,
            message: this.$t("validate.length", { from: "3", to: "20" }),
            trigger: "blur"
          }
        ],
        lastName: [
          {
            required: true,
            message: this.$t("validate.missInput", {
              input: this.$t("user.lastname").toLowerCase()
            }),
            trigger: "blur"
          },
          {
            min: 5,
            max: 20,
            message: this.$t("validate.length", { from: "5", to: "20" }),
            trigger: "blur"
          }
        ]
      }
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
          console.error(e);
          this.$router.push({ name: "user" });
        });
    },
    submitForm(formName) {
      console.groupCollapsed("Submit Form");

      this.isLoading = true;
      this.modelstate = {};
      this.$refs[formName].validate(valid => {
        if (valid) {
          console.time("Edit User");

          userSerivce
            .edit({
              ...this.editUser,
              addedGateList: this.addedGateList.toString(),
              removedGateList: this.removedGateList.toString()
            })
            .then(res => {
              if (res.status == 200) {
                this.$notify({
                  group: "main",
                  type: "success",
                  text: res.data
                });
                this.$router.push({ name: "user" });
              }
              console.log(res);
            })
            .catch(error => {
              console.log("error");

              if (error.response.status == 400) {
                this.modelstate = JSON.parse(
                  JSON.stringify(error.response.data)
                );
              }
            })
            .finally(() => {
              this.isLoading = false;
            });
          console.timeEnd("Edit User");
        } else {
          console.log("error submit!!");
          return false;
        }
      });
      console.groupEnd("Submit Form");
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
            $state.complete();
          } else {
            $state.loaded();
            $state.complete();
          }
        })
        .catch(e => {
          console.error(e);
          $state.error();
        });
    },
    handleSelectionChange(selection, item) {
      this.selectedGateList = selection;
    },
    handleSelect(selection, row) {
      setTimeout(() => {
        this.selectGate(row);
      }, 0);
    },
    handleSelectAll(selection) {
      console.groupCollapsed("handle Select All");
      console.table(selection);

      console.time("handle Select All");
      this.loader = true;
      this.removedGateList = [];
      this.addedGateList = [];

      if (selection?.length) {
        selection.forEach(gate => {
          if (!gate.checked) this.addedGateList.push(gate.id);
        });
      } else {
        this.gateList.forEach(gate => {
          if (gate.checked) {
            this.removedGateList.push(gate.id);
          }
        });
      }
      this.loader = false;

      console.groupCollapsed("Arrays");
      console.table(this.$refs["dataTable"].$refs["table"].selection);
      console.table(this.addedGateList);
      console.table(this.removedGateList);
      console.groupEnd("Arrays");
      console.timeEnd("handle Select All");
      console.groupEnd("handle Select All");
    },
    handleRowClick(row, column, event) {
      this.$refs["dataTable"].$refs["table"].toggleRowSelection(row);
      setTimeout(() => {
        this.selectGate(row);
      }, 0);
    },
    toggleSelection(rows) {
      if (rows) {
        rows.forEach(row => {
          this.$refs["dataTable"].$refs["table"].toggleRowSelection(row);
        });
      }
    },
    selectGate(row) {
      console.groupCollapsed("Select Gate");

      console.time("select Gate");

      const isCheck = this.selectedGateList.includes(row);

      if (isCheck) {
        if (row.checked) {
          this.removedGateList.splice(this.removedGateList.indexOf(row.id));
        } else {
          this.addedGateList.push(row.id);
        }
      } else {
        if (row.checked) {
          this.removedGateList.push(row.id);
        } else {
          this.addedGateList.splice(this.addedGateList.indexOf(row.id));
        }
      }

      console.timeEnd("select Gate");

      console.groupCollapsed("Arrays");
      console.table(this.selectedGateList);
      console.table(this.addedGateList);
      console.table(this.removedGateList);
      console.groupEnd("Arrays");
      console.groupEnd("Select Gate");
    }
  }
};
</script>

<style lang="scss" scoped>
.el-form--inline .el-form-item__content {
  width: 300px;
}
</style>
