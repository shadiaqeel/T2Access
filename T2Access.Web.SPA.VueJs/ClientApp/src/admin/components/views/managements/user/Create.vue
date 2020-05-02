<template>
  <div>
    <h4 style="display: inline;">Create User</h4>
    <div style="margin: 30px 40px;  ">
      <el-form
        label-position="top"
        label-width="180px"
        ref="newUserForm"
        :model="newUser"
        :rules="rules"
        status-icon
        hide-required-asterisk
        size="medium"
      >
        <el-form-item
          label="User Name"
          style="width:100%;"
          prop="username"
          :error="modelstate['UserName']"
        >
          <el-input v-model="newUser.username"></el-input>
        </el-form-item>
        <div class="row">
          <el-form-item
            label="First Name"
            class="col-md-6"
            prop="firstname"
            :error="modelstate['FirstName']"
          >
            <el-input v-model="newUser.firstname"></el-input>
          </el-form-item>
          <el-form-item
            label="Last Name"
            class="col-md-6"
            prop="lastname"
            :error="modelstate['LastName']"
          >
            <el-input v-model="newUser.lastname"></el-input>
          </el-form-item>
        </div>
        <div class="row">
          <el-form-item
            label="Password"
            class="col-md-6"
            prop="password"
            :error="modelstate['Password']"
          >
            <el-input v-model="newUser.password" show-password></el-input>
          </el-form-item>
          <el-form-item
            label="Confirm Password"
            class="col-md-6"
            prop="confirmPassword"
            :error="modelstate['ConfirmPassword']"
          >
            <el-input
              v-model="newUser.confirmPassword"
              show-password
            ></el-input>
          </el-form-item>
        </div>

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
            :height="250"
            rowKey="id"
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
              :reserve-selection="true"
              width="55"
              property="checked"
            ></el-table-column>

            <el-table-column
              min-width="100"
              label="Arabic Name"
              property="nameAr"
              sortable
            ></el-table-column>

            <el-table-column
              min-width="100"
              label="Engilsh Name"
              property="nameEn"
              sortable
            ></el-table-column>
          </Datatable>
        </el-card>

        <div class="mt centered">
          <el-form-item>
            <el-button type="primary" @click="submitForm('newUserForm')"
              >Create</el-button
            >
            <el-button @click="$router.push({ name: 'user' })">Exit</el-button>
          </el-form-item>
        </div>
      </el-form>
    </div>
  </div>
</template>

<script>
import Datatable from 'admin/components/elements/Datatable';

import gateSerivce from 'admin/services/gate-service';
import userSerivce from 'admin/services/user-service';

export default {
  name: 'CreateUser',
  components: {
    Datatable
  },
  data() {
    var validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('Please input the password'));
      } else {
        if (this.newUser.confirmPassword !== '') {
          this.$refs.newUserForm.validateField('confirmPassword');
        }
        callback();
      }
    };
    var validatePass2 = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('Please input the password again'));
      } else if (value !== this.newUser.password) {
        callback(
          new Error('The password and confirmation password do not match!')
        );
      } else {
        callback();
      }
    };

    return {
      page: 1,
      loader: false,
      gateList: [],
      selectedList: [],
      modelstate: {},
      newUser: {
        username: '',
        firstname: '',
        lastname: '',
        password: '',
        confirmPassword: '',
        addedGateList: ''
      },
      rules: {
        username: [
          {
            required: true,
            message: 'Please input user name',
            trigger: 'blur'
          },
          {
            min: 8,
            max: 20,
            message: 'Length should be 8 to 20',
            trigger: 'blur'
          }
        ],
        firstname: [
          {
            required: true,
            message: 'Please input first name',
            trigger: 'blur'
          },
          {
            min: 3,
            max: 20,
            message: 'Length should be 3 to 20',
            trigger: 'blur'
          }
        ],
        lastname: [
          {
            required: true,
            message: 'Please input last name',
            trigger: 'blur'
          },
          {
            min: 5,
            max: 20,
            message: 'Length should be 5 to 20',
            trigger: 'blur'
          }
        ],
        password: [
          {
            min: 8,
            max: 20,
            message: 'Length should be 8 to 20',
            trigger: 'blur'
          },
          { validator: validatePass, trigger: 'blur' }
        ],
        confirmPassword: [
          {
            min: 8,
            max: 20,
            message: 'Length should be 8 to 20',
            trigger: 'blur'
          },
          { validator: validatePass2, trigger: 'blur' }
        ]
      }
    };
  },
  created() {
    // this.loadGateList();
  },
  methods: {
    submitForm(formName) {
      this.modelstate = {};

      this.$refs[formName].validate(valid => {
        if (valid) {
          this.newUser.addedGateList = this.selectedList
            .map(gate => gate.id)
            .toString();
          userSerivce
            .create(this.newUser)
            .then(res => {
              if (res.status == 200) {
                this.$notify({
                  group: 'main',
                  type: 'success',
                  text: res.data
                });
                this.$router.push({ name: 'user' });
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
          console.log('error submit!!');
          return false;
        }
      });
    },
    infiniteHandler($state) {
      var start = (this.page - 1) * 10;
      gateSerivce
        .fetch({
          start: start,
          length: 10
        })
        .then(res => {
          if (res.status == 200 && res.data.list.length) {
            this.page += 1;
            this.gateList = this.gateList.concat(res.data.list);
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
    handleSelectionChange(selection, item) {
      this.selectedGateList = selection;
    },
    handleSelect(selection, row) {
      // setTimeout(() => {
      //   this.selectGate(row);
      // }, 0);
    },
    handleSelectAll(selection) {
      console.groupCollapsed('handle Select All');
      console.table(selection);

      console.time('handle Select All');
      this.loader = true;
      this.selectedList = [];

      if (selection?.length) {
        selection.forEach(gate => {
          this.selectedList.push(gate.id);
        });
      }

      this.loader = false;

      console.groupCollapsed('Arrays');
      console.table(this.$refs['dataTable'].$refs['table'].selection);
      console.table(this.addedGateList);
      console.table(this.removedGateList);
      console.groupEnd('Arrays');
      console.timeEnd('handle Select All');
      console.groupEnd('handle Select All');
    },
    handleRowClick(row, column, event) {
      this.$refs['dataTable'].$refs['table'].toggleRowSelection(row);
      // setTimeout(() => {
      //   this.selectGate(row);
      // }, 0);
    },
    toggleSelection(rows) {
      if (rows) {
        rows.forEach(row => {
          this.$refs['dataTable'].$refs['table'].toggleRowSelection(row);
        });
      }
    }
    // selectGate(row) {
    //   console.groupCollapsed("Select Gate");

    //   console.time("select Gate");

    //   const isCheck = this.selectedGateList.includes(row);

    //   if (isCheck) {
    //     if (row.checked) {
    //       this.removedGateList.splice(this.removedGateList.indexOf(row.id));
    //     } else {
    //       this.addedGateList.push(row.id);
    //     }
    //   } else {
    //     if (row.checked) {
    //       this.removedGateList.push(row.id);
    //     } else {
    //       this.addedGateList.splice(this.addedGateList.indexOf(row.id));
    //     }
    //   }

    //   console.timeEnd("select Gate");

    //   console.groupCollapsed("Arrays");
    //   console.table(this.selectedGateList);
    //   console.table(this.addedGateList);
    //   console.table(this.removedGateList);
    //   console.groupEnd("Arrays");
    //   console.groupEnd("Select Gate");
    // }
  }
};
</script>

<style lang="scss" scoped></style>
