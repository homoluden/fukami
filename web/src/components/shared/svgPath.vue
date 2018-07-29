<template>
  <path
    :d="d"
    class="svg-path"/>
</template>

<script>
export default {
  props: {
    points: {
      type: Array,
      default() {
        return [[0, 0], [100, 0], [100, 100], [0, 100]];
      },
    },
  },
  data() {
    return {
      d: `M${this.points.join('L')}Z`,
    };
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
