<template>
  <path
    :d="d"
    :style="{
      cursor: (activeCell ? 'pointer' : null),
      fill: (activeCell ? activeCell.color : null),
      opacity: (activeCell ? 1.0 : null)
    }"
    :class="{'svg-path': true, 'active-cell': activeCell}"
    @click="cellClicked"/>
</template>

<script>
import {
  isFunction,
  isObject,
} from 'lodash';

export default {
  props: {
    points: {
      type: Array,
      default() {
        return [[0, 0], [100, 0], [100, 100], [0, 100]];
      },
    },
    activeCell: {
      type: Object,
      default: null,
    },
    previewCell: {
      type: Function,
      default: null,
    },
  },
  data() {
    return {
      d: `M${this.points.join('L')}Z`,
    };
  },
  methods: {
    cellClicked() {
      if (!isObject(this.activeCell) || !isFunction(this.previewCell)) {
        return;
      }

      this.previewCell(this.activeCell);
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
    stroke: md-get-palette-color(grey, 900);
    stroke-width: 1;
    opacity: 0.1;

    &:hover {
      fill: $fill-hover;
      stroke: $stroke;
      stroke-width: 1;
      opacity: 1;
    }
    &.active-cell:hover {
      stroke-width: 4;
    }
  }
</style>
