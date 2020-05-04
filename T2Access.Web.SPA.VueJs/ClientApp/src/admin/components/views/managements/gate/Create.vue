<template>
  <el-dialog
    :title="$t('gate.add')"
    :visible.sync="dialogFormVisible"
    @closed="$router.push({ name: 'gate' })"
    :center="true"
  >
    <el-form
      :model="newGate"
      label-position="top"
      label-width="180px"
      ref="newGateForm"
      size="medium"
      :rules="rules"
      status-icon
      hide-required-asterisk
    >
      <el-form-item
        :label="$t('gate.username')"
        :label-width="formLabelWidth"
        prop="username"
        :error="modelstate['UserName']"
      >
        <el-input v-model="newGate.username" autocomplete="off"></el-input>
      </el-form-item>
      <div class="row">
        <div class="col-md-6">
          <el-form-item
            :label="$t('gate.nameAr')"
            :label-width="formLabelWidth"
            prop="nameAr"
            :error="modelstate['NameAr']"
          >
            <el-input v-model="newGate.nameAr" autocomplete="off"></el-input>
          </el-form-item>
        </div>
        <div class="col-md-6">
          <el-form-item
            :label="$t('gate.nameEn')"
            :label-width="formLabelWidth"
            prop="nameEn"
            :error="modelstate['NameEn']"
          >
            <el-input v-model="newGate.nameEn" autocomplete="off"></el-input>
          </el-form-item>
        </div>
      </div>
      <div class="row">
        <div class="col-md-6">
          <el-form-item
            :label="$t('password')"
            :label-width="formLabelWidth"
            prop="password"
            :error="modelstate['Password']"
          >
            <el-input v-model="newGate.password" show-password></el-input>
          </el-form-item>
        </div>

        <div class="col-md-6">
          <el-form-item
            :label="$t('confirmPassword')"
            :label-width="formLabelWidth"
            prop="confirmPassword"
            :error="modelstate['ConfirmPassword']"
          >
            <el-input v-model="newGate.confirmPassword" show-password></el-input>
          </el-form-item>
        </div>
      </div>
    </el-form>
    <span slot="footer" class="dialog-footer">
      <el-button @click="dialogFormVisible = false">
        {{
        $t('cancel')
        }}
      </el-button>
      <el-button type="primary" @click="submitForm('newGateForm')">
        {{
        $t('add')
        }}
      </el-button>
    </span>
  </el-dialog>
</template>

<script>
import gateSerivce from "admin/services/gate-service";

export default {
  name: "CreateGate",
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
        if (this.newGate.checkPass !== "") {
          this.$refs.newGateForm.validateField("confirmPassword");
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
      } else if (value !== this.newGate.password) {
        callback(new Error(this.$t("validate.missMatchPass")));
      } else {
        callback();
      }
    };

    return {
      dialogFormVisible: true,
      formLabelWidth: "120px",
      modelstate: {},
      newGate: {
        username: "",
        nameAr: "",
        nameEn: "",
        password: "",
        ConfirmPassword: ""
      },
      rules: {
        username: [
          {
            required: true,
            message: this.$t("validate.missInput", {
              input: this.$t("gate.username").toLowerCase()
            }),
            trigger: "blur"
          },
          {
            min: 7,
            max: 20,
            message: this.$t("validate.length", { from: "8", to: "20" }),
            trigger: "blur"
          }
        ],
        nameAr: [
          {
            required: true,
            message: this.$t("validate.missInput", {
              input: this.$t("gate.nameAr").toLowerCase()
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
        nameEn: [
          {
            required: true,
            message: this.$t("validate.missInput", {
              input: this.$t("gate.nameEn").toLowerCase()
            }),
            trigger: "blur"
          },
          {
            min: 5,
            max: 20,
            message: this.$t("validate.length", { from: "5", to: "20" }),
            trigger: "blur"
          }
        ],
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
  methods: {
    submitForm(formName) {
      this.modelstate = {};

      this.$refs[formName].validate(valid => {
        if (valid) {
          gateSerivce
            .create(this.newGate)
            .then(res => {
              if (res.status == 200) {
                console.log(res);
                this.$notify({
                  group: "main",
                  type: "success",
                  text: res.data
                });
                this.dialogFormVisible = false;
              }
            })
            .catch(error => {
              console.log(error.response);

              if (error.response.status == 400) {
                this.modelstate = JSON.parse(
                  JSON.stringify(error.response.data)
                );
              }

              this.$notify({
                group: "main",
                type: "error",
                text: error
              });
            });
        } else {
          this.$notify({
            group: "main",
            type: "error",
            text: "error submit!!"
          });

          return false;
        }
      });
    }
  }
};
</script>

<style lang="sass" scoped></style>
