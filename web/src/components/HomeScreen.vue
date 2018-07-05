<template>
  <div>
    <div class="viewport">
      <md-bottom-bar class="md-alignment-top-center"
        md-type="shift" :md-theme="'bottom-bar-' + theme">
        <md-bottom-bar-item md-label="Home" md-icon="home" @click="theme = 'teal'"></md-bottom-bar-item>
        <md-bottom-bar-item md-label="Pages" md-icon="pages" @click="theme = 'orange'"></md-bottom-bar-item>
        <md-bottom-bar-item md-label="Favorites" md-icon="favorite" @click="theme = 'red'"></md-bottom-bar-item>
      </md-bottom-bar>
    </div>
  </div>
</template>

<script>
export default {
  name: 'HomeScreen',
  data() {
    return {
      theme: 'teal',
    };
  },
};
</script>

<style lang="scss">
@import "~vue-material/dist/theme/engine";

$active-border-color: md-get-palette-color(bluegrey, 500);

.viewport {
  width: 100vw;
  height: 100vh;
  display: inline-flex;
  align-items: flex-end;
  overflow: hidden;
  border: 1px solid rgba(#000, .26);
  background: rgba(#000, .06);

  .md-bottom-bar .md-bottom-bar-item.md-active {
    width: initial;
    border-top: 5px $active-border-color solid;
  }

  .md-bottom-bar>.md-ripple {
    justify-content: center;
  }
}

</style>

<style lang="scss">
  @import "~vue-material/dist/theme/engine";

  @include md-register-theme("bottom-bar-teal", (
    primary: md-get-palette-color(teal, A200)
  ));

  @include md-register-theme("bottom-bar-orange", (
    primary: md-get-palette-color(orange, A200)
  ));

  @include md-register-theme("bottom-bar-blue", (
    primary: md-get-palette-color(blue, A200),
    accent: md-get-palette-color(red, A200)
  ));

  @include md-register-theme("bottom-bar-red", (
    primary: md-get-palette-color(red, A200)
  ));

  @import "~vue-material/dist/theme/all";
</style>
