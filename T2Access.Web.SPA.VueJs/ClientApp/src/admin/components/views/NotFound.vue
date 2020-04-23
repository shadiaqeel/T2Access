<template>
<div id="not-found">
  <div class="shrug">
    <p class="title"><b>404 - Page not found.</b></p><span class="left-arm">¯\_</span>(ツ)<span class="right-arm">_/¯</span>
    <h6>The page you trying to reach may not exist.</h6>
    <el-button type="text" @click="goToHome">Go back to home</el-button>
  </div>
</div>
</template>

<script>
export default {
  name: "NotFound",

  methods: {
    goToHome() {
      this.$router.push({ name: "home" });
    }
  }
};
</script>

<style lang="scss" scoped>
body{
	position: absolute;
	width: 100%;
	height: 100%;
	overflow: hidden;
	text-align: center;

	.shrug{
		text-align: center;
		display: inline-block;
		position: absolute;
		top: 50%;
		left: 50%;
		transform: translate(-50%,-50%);
		font-size: 50px;
		white-space: nowrap;
		padding: 10px;
		color: #b4b4bf;

		.title{
			margin-bottom: 20px;
		}

		h6{
			margin-top: 40px;
		}


		.left-arm,
		.right-arm{
			display: inline-block;
			transform: rotate(0deg);
		}


		.left-arm{
			animation: 5s infinite left-arm;
		}


		.right-arm{
			animation: 5s infinite right-arm
		}
	}
}


	@keyframes left-arm{
		10%{
			transform: rotate(-20deg);
		}

		30%{
			transform: rotate(-20deg);
		}

		50%{
			transform: rotate(0deg);
		}
	}

	@keyframes right-arm{
		10%{
			transform: rotate(20deg);
		}

		30%{
			transform: rotate(20deg);
		}

		50%{
			transform: rotate(0deg);
		}
	}

</style>
