<template>
  <path
    :d="data"
    class="svg-path"/>
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
    stroke: md-get-palette-color(grey, 900);
    stroke-width: 1;
    opacity: 0.1;

    &:hover {
      fill: $fill-hover;
      stroke: $stroke;
      stroke-width: 4;
      opacity: 1;
    }
  }
</style>
