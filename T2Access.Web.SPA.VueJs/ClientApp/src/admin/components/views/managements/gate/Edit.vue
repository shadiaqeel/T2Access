<template>
  <el-dialog
    title="Edit GATE"
    :visible.sync="dialogFormVisible"
    @closed="$router.push({ name: 'gate' })"
    :center="true"
  >
    <el-form
      :model="editGate"
      label-position="top"
      ref="editGateForm"
      size="medium"
      :rules="rules"
    >
      <el-form-item
        label="User Name"
        :label-width="formLabelWidth"
        :error="modelstate['UserName']"
      >
        <el-input
          :disabled="true"
          v-model="editGate.userName"
          autocomplete="off"
          prop="username"
        ></el-input>
      </el-form-item>
      <div class="row">
        <div class="col-md-6">
          <el-form-item
            label="Arabic Name"
            :label-width="formLabelWidth"
            prop="nameAr"
            :error="modelstate['NameAr']"
          >
            <el-input v-model="editGate.nameAr" autocomplete="off"></el-input>
          </el-form-item>
        </div>
        <div class="col-md-6">
          <el-form-item
            label="Engilsh Name"
            :label-width="formLabelWidth"
            prop="nameEn"
            :error="modelstate['NameEn']"
          >
            <el-input v-model="editGate.nameEn" autocomplete="off"></el-input>
          </el-form-item>
        </div>
      </div>
      <div class="row">
        <div class="col-md-6">
          <el-form-item label="Status" :label-width="formLabelWidth">
            <el-select
              size="small"
              style="width:150px"
              v-model="editGate.status"
              value-key="filter.status"
              @clear="editGate.status = null"
              placeholder="Status"
            >
              <el-option
                v-for="(status, index) in gateStatus"
                :key="index"
                :label="status.label"
                :value="index"
              ></el-option>
            </el-select>
          </el-form-item>
        </div>
      </div>
    </el-form>
    <span slot="footer" class="dialog-footer">
      <el-button @click="dialogFormVisible = false">Cancel</el-button>
      <el-button type="primary" @click="submitForm('editGateForm')"
        >Edit</el-button
      >
    </span>
  </el-dialog>
</template>

<script>
import { gateStatus } from 'admin/types/status';
import { mapGetters } from 'vuex';
// import gateSerivce from "admin/services/gate-service";

export default {
  name: 'EditGate',
  props: ['userId'],
  data() {
    return {
      gateStatus: gateStatus,
      dialogFormVisible: true,
      formLabelWidth: '120px',
      modelstate: {},
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
        nameAr: [
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
        nameEn: [
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
          }
          // { validator: validatePass, trigger: "blur" }
        ],
        confirmPassword: [
          {
            min: 8,
            max: 20,
            message: 'Length should be 8 to 20',
            trigger: 'blur'
          }
          // { validator: validatePass2, trigger: "blur" }
        ]
      }
    };
  },
  computed: {
    ...mapGetters('gate', ['editGate'])
  },
  methods: {
    submitForm(formName) {
      this.modelstate = {};

      this.$refs[formName].validate(valid => {
        if (valid) {
          console.log(this.editGate);
          this.$store
            .dispatch('gate/edit', this.editGate)
            .then(message => {
              this.$notify({
                group: 'main',
                type: 'success',
                text: message
              });
              this.dialogFormVisible = false;
            })
            .catch(error => {
              if (error.response.status == 400) {
                this.modelstate = JSON.parse(
                  JSON.stringify(error.response.data)
                );
              }
            });
        } else {
          this.$notify({
            group: 'main',
            type: 'error',
            text: 'error submit!!'
          });

          return false;
        }
      });
    }
  }
};
</script>

<style lang="sass" scoped></style>
