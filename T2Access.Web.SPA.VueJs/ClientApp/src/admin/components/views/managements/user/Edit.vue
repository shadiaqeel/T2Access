<template >
  <div>
    <h4 style="display: inline;">Edit User</h4>
    <div style="margin: 30px 40px; ">
      <el-form label-position="left" label-width="100px" :model="user" :inline="true">
        <el-form-item label="User Name" style="width:100%;">
          <el-input :disabled="true" v-model="user.userName"></el-input>
        </el-form-item>
        <div class="demo-input-suffix">
          <el-form-item label="First Name">
            <el-input v-model="user.firstName"></el-input>
          </el-form-item>
          <el-form-item label="Last Name">
            <el-input v-model="user.lastName"></el-input>
          </el-form-item>
        </div>
        <el-form-item label="Status">
          <el-select v-model="user.status" value-key="user.status" placeholder="Status">
            <el-option
              v-for="(status, index) in userStatus"
              :key="index"
              :label="status.label"
              :value="index"
            ></el-option>
          </el-select>
        </el-form-item>
        <div class="mt centered">
          <el-form-item>
            <el-button type="primary" @click="submitForm('ruleForm')">Edit</el-button>
            <el-button @click="$router.push({name:'user'})">Exit</el-button>
          </el-form-item>
        </div>
      </el-form>
    </div>
  </div>
</template>

<script>
// import { mapGetters } from "vuex";
// import { Notification } from "admin/utils/helper/notification";
import { userStatus } from "admin/types/status";
export default {
  name: "EditUser",
  props: ["userId"],
  data() {
    return {
      user: null,
      userStatus: userStatus
    };
  },

  created() {
    this.user = Object.assign({}, this.$store.getters.getUser(this.userId));
  }
};
</script>

<style lang="scss" scoped>
.el-form--inline .el-form-item__content {
  width:300px;
}
</style>
