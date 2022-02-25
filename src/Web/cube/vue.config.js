const fs = require('fs')
const path = require('path')

//const baseFolder =
//    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
//        ? `${process.env.APPDATA}/ASP.NET/https`
//        : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : "cube";

if (!certificateName) {
    console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
    process.exit(-1);
}

const baseFolder = "certs";

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

module.exports = {
    devServer: {
        //https: {
        //    key: fs.readFileSync(keyFilePath),
        //    cert: fs.readFileSync(certFilePath),
        //},
        disableHostCheck: true,
        proxy: {

            /************************************* Development *********************************/
            /*Connect to local Microservices*/

            //'^/Board': {
            //    target: 'https://localhost:5000'
            //},
            //'^/Comment': {
            //    target: 'https://localhost:5000'
            //},
            //'^/User': {
            //    target: 'https://localhost:4000'
            //},
            //'^/Authorize': {
            //    target: 'https://localhost:3000',
            //},

            /*Connect to local microservices via Gateway Service*/

            // '^/Board': {
            //     target: 'http://localhost:9070'
            // },
            // '^/Comment': {
            //     target: 'http://localhost:9070'
            // },
            // '^/User': {
            //     target: 'http://localhost:9070'
            // },
            // '^/Authorize': {
            //     target: 'http://localhost:9070'
            // }

            /************************************* Production *********************************/
            /*Connect to remote Microservices directly*/

            //'^/Board': {
            //    target: 'https://10.63.224.86:5000'
            //},
            //'^/Comment': {
            //    target: 'https://10.63.224.86:5000'
            //},
            //'^/User': {
            //    target: 'https://10.63.224.86:4000'
            //},
            //'^/Authorize': {
            //    target: 'https://10.63.224.86:3000'
            //}

            /*Connect to remote microservices via Gateway Service*/

            '^/Board': {
              target: 'http://10.63.224.86:9070'
            },
            '^/Comment': {
              target: 'http://10.63.224.86:9070'
            },
            '^/User': {
              target: 'http://10.63.224.86:9070'
            },
            '^/Authorize': {
              target: 'http://10.63.224.86:9070'
            }
        },
        port: 80
    }
}