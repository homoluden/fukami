<template>
  <svg class="svg-canvas" :style="{ width: width, height: height }">
    <slot></slot>
  </svg>
</template>

<script>
export default {
  props: {
    width: [String, Number],
    height: [String, Number],
  },
};
</script>

<style>
</style>
