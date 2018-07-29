import {
  isArray,
} from 'lodash';

function fkRound(number, fraction = 4) {
  let result = number;

  for (let i = 0; i < fraction; i++) {
    result *= 10;
  }

  result = Math.round(result);

  for (let i = 0; i < fraction; i++) {
    result /= 10;
  }

  return result;
}

function fkRoundArray(array, fraction = 4) {
  if (!isArray(array) || array.length === 0) {
    return [];
  }

  if (isArray(array[0])) {
    // Let's round nested arrays
    const roundedArrays = array.map(arr => fkRoundArray(arr));
    return roundedArrays;
  }

  return array.map(num => fkRound(num, fraction));
}

const fkMath = {
  round: fkRound,
  roundArray: fkRoundArray,
};

window.fkMath = fkMath;
export default fkMath;
