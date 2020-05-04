<template>
  <div id="data-table">
    <h3 class="title-section" v-if="title">{{ title }}</h3>
    <el-table
      :row-key="rowKey"
      ref="table"
      v-loading="loader"
      element-loading-text="Loading..."
      element-loading-spinner="el-icon-loading"
      element-loading-background="rgba(0, 0, 0, 0.8)"
      :data="data"
      :height="height"
      stripe
      :border="border"
      style="width: 100%"
      highlight-current-row
      @select-all="handleSelectAll"
      @select="handleSelect"
      @selection-change="handleSelectionChange"
      @current-change="handleCurrentChange"
      @row-click="handleRowClick"
      @sort-change="handleSortChange"
    >
      <div v-if="columns">
        <div class="columns" v-for="(column, index) in columns" :key="index">
          <el-table-column
            :type="column.type"
            :label="column.label"
            :property="column.name"
            :show-overflow-tooltip="column.showOverflowTooltip"
          ></el-table-column>
        </div>
      </div>
      <slot v-else />

      <infinite-loading
        v-if="infiniteLoading"
        slot="append"
        @infinite="infiniteHandler"
        force-use-infinite-wrapper=".el-table__body-wrapper"
        :spinner="spinner"
      >
        <div slot="no-more">{{ noMoreMessage }}</div>
        <div slot="no-results">{{ noResultsMessage }}</div>
        <div slot="error" slot-scope="{ trigger }">
          Opps, something went wrong :(
          <a href="javascript:;" @click="trigger">Retry</a>
        </div>
      </infinite-loading>
    </el-table>
    <el-pagination
      class="pagination"
      v-if="pagination"
      :total="totalInServer"
      :page-size="numDataPerPage"
      @current-change="updatePage"
      :currentPage="currentPage"
      :layout="paginationLayout"
      @prev-click="prevPage"
      @next-click="nextPage"
      @size-change="sizeChange"
      background
    ></el-pagination>
  </div>
</template>

<script>
import InfiniteLoading from "vue-infinite-loading";

export default {
  name: "DataTable",
  components: { InfiniteLoading },
  props: {
    title: {
      type: String,
      default: null
    },

    data: {
      type: Array,
      required: true
    },
    rowKey: {
      required: false
    },
    totalInServer: {
      type: Number,
      default: 0
    },

    columns: {
      type: Array,
      required: false
    },

    pagination: {
      type: Boolean,
      default: false
    },

    paginationLayout: {
      type: String,
      default: "total, sizes, prev, pager, next, jumper"
    },

    height: {
      type: Number,
      default: null
    },

    currentPage: {
      type: Number,
      default: 1
    },

    loader: {
      type: Boolean,
      default: true
    },

    numPerPage: {
      type: Number,
      default: 10
    },
    border: {
      type: Boolean,
      default: false
    },
    infiniteLoading: {
      type: Boolean,
      default: false
    },
    spinner: {
      type: String,
      default: "default"
    },
    noMoreMessage: {
      type: String,
      default: "No more message"
    },
    noResultsMessage: {
      type: String,
      default: "No more message"
    }
  },

  data() {
    return {
      dataToTable: [],
      numDataPerPage: this.numPerPage,
      numMaxPage: 0,
      selectedData: null,
      actualPage: 0
    };
  },

  created() {
    this.actualPage = this.currentPage;
  },

  methods: {
    prevPage(page) {
      this.$emit("current-page", page);
    },

    nextPage(page) {
      this.$emit("current-page", page);
    },

    sizeChange(size) {
      this.$emit("size-table", size);
    },

    updatePage(page) {
      this.$emit("current-page", page);
    },
    infiniteHandler($state) {
      this.$emit("infinite-handler", $state);
    },
    handleCurrentChange(val) {
      this.$emit("current-change", val);
    },
    handleSelect(selected, row) {
      this.selectedData = selected;

      this.$emit("select", selected, row);
    },
    handleSelectionChange(selected, row) {
      this.selectedData = selected;

      this.$emit("selection-change", selected, row);
    },
    handleSelectAll(selected) {
      this.selectedData = selected;
      this.$emit("select-all", selected);
    },
    handleRowClick(row, column, event) {
      this.$emit("row-click", row, column, event);
    },
    handleSortChange(column, prop, order) {
      this.$emit("sort-change", column, prop, order);
    }
  }
};
</script>

<style lang="scss" scoped>
// @import "../../styles/tools/variables"

#data-table {
  position: relative;
  padding: 20px;
  border-radius: 4px;

  .title-section {
    margin-bottom: 20px;
  }

  .pagination {
    margin-top: 20px;
  }
}
</style>
