<template>
  <path class="svg-path" :d="data"/>
</template>

<script>
export default {
  props: {
    data: {
      type: String,
      required: true,
      default: 'M0,0L100,0L100,100L0,100L0,0Z',
    },
  },
};
</script>

<style lang="scss">
  @import "~vue-material/dist/theme/engine";
  $fill-norm: md-get-palette-color(grey, 800);
  $fill-hover: md-get-palette-color(grey, 700);
  $stroke: md-get-palette-color(amber, 300);

  path.svg-path {
    fill: $fill-norm;
    cursor: pointer;

    &:hover {
      fill: $fill-hover;
      border: 1px solid #eee;
      stroke: $stroke;
      stroke-width: 1;
    }
  }
</style>
