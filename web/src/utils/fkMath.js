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

const fkMath = {
  round: fkRound,
};

window.fkMath = fkMath;
export default fkMath;
