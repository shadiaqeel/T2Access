<template>
  <el-dialog
    title="Reset Password"
    :visible.sync="dialogFormVisible"
    @closed="$router.push({ name: 'gate' })"
    :center="true"
  >
    <el-form
      :model="model"
      label-position="top"
      ref="resetPasswordForm"
      size="medium"
      :rules="rules"
    >
      <el-form-item
        :label="$t('gate.username')"
        :label-width="formLabelWidth"
        :error="modelstate['UserName']"
      >
        <el-input :disabled="true" v-model="gate.userName" autocomplete="off" prop="userName"></el-input>
      </el-form-item>
      <div class="row">
        <div class="col-md-6">
          <el-form-item
            :label="$t('password')"
            :label-width="formLabelWidth"
            prop="password"
            :error="modelstate['Password']"
          >
            <el-input v-model="model.password" show-password></el-input>
          </el-form-item>
        </div>
        <div class="col-md-6">
          <el-form-item
            :label="$t('confirmPassword')"
            :label-width="formLabelWidth"
            prop="confirmPassword"
            :error="modelstate['ConfirmPassword']"
          >
            <el-input v-model="model.confirmPassword" show-password></el-input>
          </el-form-item>
        </div>
      </div>
    </el-form>
    <span slot="footer" class="dialog-footer">
      <el-button @click="dialogFormVisible = false">{{$t('cancel')}}</el-button>
      <el-button
        :loading="isLoading"
        type="primary"
        @click="submitForm('resetPasswordForm')"
      >{{$t('reset')}}</el-button>
    </span>
  </el-dialog>
</template>

<script>
import gateSerivce from "admin/services/gate-service";

export default {
  name: "ResetPassword",
  props: ["gate"],
  data() {
    var validatePass = (rule, value, callback) => {
      if (value === "") {
        callback(
          new Error(
            this.$t("validate.missInput", {
              input: this.$t("password").toLowerCase()
            })
          )
        );
      } else {
        if (this.model.confirmPassword !== "") {
          this.$refs.resetPasswordForm.validateField("confirmPassword");
        }
        callback();
      }
    };
    var validatePass2 = (rule, value, callback) => {
      if (value === "") {
        callback(
          new Error(
            this.$t("validate.missInput", {
              input: this.$t("confirmPassword").toLowerCase()
            })
          )
        );
      } else if (value !== this.model.password) {
        callback(new Error(this.$t("validate.missMatchPass")));
      } else {
        callback();
      }
    };

    return {
      dialogFormVisible: true,
      formLabelWidth: "130px",
      isLoading: false,
      modelstate: {},
      model: {
        password: "",
        confirmPassword: ""
      },
      rules: {
        password: [
          {
            min: 8,
            max: 20,
            message: this.$t("validate.length", { from: "8", to: "20" }),
            trigger: "blur"
          },
          { validator: validatePass, trigger: "blur" }
        ],
        confirmPassword: [
          {
            min: 8,
            max: 20,
            message: this.$t("validate.length", { from: "8", to: "20" }),
            trigger: "blur"
          },
          { validator: validatePass2, trigger: "blur" }
        ]
      }
    };
  },

  mounted() {
    console.log(this.gate);

    if (!(this.gate.id && this.gate.userName))
      this.$router.push({ name: "gate" });
  },
  methods: {
    submitForm(formName) {
      console.group("Submit Reset Password User Form");
      this.isLoading = true;
      this.modelstate = {};
      this.$refs[formName].validate(valid => {
        if (valid) {
          console.log({ ...this.gate, ...this.model });
          gateSerivce
            .resetPassword({ ...this.gate, ...this.model })
            .then(res => {
              console.log("Api response :", res);
              if (res.status == 200) {
                this.$notify({
                  group: "main",
                  type: "success",
                  text: res.data
                });
                this.dialogFormVisible = false;
              }
            })
            .catch(error => {
              if (error.response.status == 400) {
                this.modelstate = JSON.parse(
                  JSON.stringify(error.response.data)
                );
              }
              console.log(error);
            })
            .finally(() => {
              this.isLoading = false;
            });
        } else {
          this.$notify({
            group: "main",
            type: "error",
            text: "error submit!!"
          });
          this.isLoading = false;
          console.groupEnd("Submit Reset Password User Form");
          return false;
        }
      });
    }
  }
};
</script>

<style lang="sass" scoped></style>
