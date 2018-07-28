<template>
  <svg
    :style="{ width: width, height: height }"
    class="svg-canvas">
    <slot/>
  </svg>
</template>

<script>
export default {
  props: {
    width: { type: [String, Number], default: '100%' },
    height: { type: [String, Number], default: '100%' },
  },
};
</script>

<style>
</style>
