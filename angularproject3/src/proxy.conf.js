const PROXY_CONFIG = [
  {
    context: [
      "/Restful",
    ],
    target: "https://localhost:7294",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
