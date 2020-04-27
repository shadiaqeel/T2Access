<template >
  <div>
    <h4 style="display: inline;">Edit User</h4>
    <div style="margin: 30px 40px;  " v-if="editUser">
      <el-form label-position="left" label-width="100px" :model="editUser">
        <el-form-item label="User Name" style="width:100%;">
          <el-input :disabled="true" v-model="editUser.userName"></el-input>
        </el-form-item>
        <div class="row">
          <el-form-item label="First Name" class="col-md-6">
            <el-input v-model="editUser.firstName"></el-input>
          </el-form-item>
          <el-form-item label="Last Name" class="col-md-6">
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
// import { mapGetters } from "vuex";
// import { Notification } from "admin/utils/helper/notification";
import { userStatus } from "admin/types/status";
import { mapGetters } from "vuex";

export default {
  name: "EditUser",
  props: ["userId"],
  data() {
    return {
      userStatus: userStatus
    };
  },

  computed: {
    ...mapGetters(["editUser"])
  },
  mounted() {
    if (!this.user) {
      this.fetchUser();
    }
  },
  methods: {
    fetchUser() {
      return this.$store
        .dispatch("fetchUser", this.$route.params.userId)
        .catch(e => {
          console.log(e);
          this.$router.push({ name: "user" });
        });
    },
    editUser() {}
  }
};
</script>

<style lang="scss" scoped>
.el-form--inline .el-form-item__content {
  width: 300px;
}
</style>
