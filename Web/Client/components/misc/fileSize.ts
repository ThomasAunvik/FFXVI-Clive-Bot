export const humanFileSize = (bytes: number, si = false, dp = 1) => {
  const thresh = si ? 1000 : 1024;

  if (Math.abs(bytes) < thresh) {
    return `${bytes} B`;
  }

  let editBytes = bytes;

  const units = si
    ? ["kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"]
    : ["KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"];
  let u = -1;
  const r = 10 ** dp;

  do {
    editBytes /= thresh;
    ++u;
  } while (
    Math.round(Math.abs(editBytes) * r) / r >= thresh &&
    u < units.length - 1
  );

  return `${editBytes.toFixed(dp)} ${units[u]}`;
};

export const millisecondsToStr = (milliseconds: number) => {
  // TIP: to find current time in milliseconds, use:
  // var  current_time_milliseconds = new Date().getTime();

  function numberEnding(number: number) {
    return number > 1 ? "s" : "";
  }

  let temp = Math.floor(milliseconds / 1000);
  const years = Math.floor(temp / 31536000);
  if (years) {
    return `${years} year${numberEnding(years)}`;
  }
  //TODO: Months! Maybe weeks?
  temp = temp % 31536000;
  const days = Math.floor(temp / 86400);
  if (days) {
    return `${days} day${numberEnding(days)}`;
  }
  temp = temp % 86400;
  const hours = Math.floor(temp / 3600);
  if (hours) {
    return `${hours} hour${numberEnding(hours)}`;
  }
  temp = temp % 3600;
  const minutes = Math.floor(temp / 60);
  if (minutes) {
    return `${minutes} minute${numberEnding(minutes)}`;
  }
  const seconds = temp % 60;
  if (seconds) {
    return `${seconds} second${numberEnding(seconds)}`;
  }
  return "less than a second"; //'just now' //or other string you like;
};
