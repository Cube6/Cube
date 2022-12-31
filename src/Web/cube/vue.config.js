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
    configureWebpack: {
        devtool: 'souce-map'
    },
    devServer: {
        //https: {
        //    key: fs.readFileSync(keyFilePath),
        //    cert: fs.readFileSync(certFilePath),
        //},
        disableHostCheck: true,
        proxy: {

            /************************************* Development *********************************/
            /*Connect to local Microservices*/

            // '^/Project': {
            //    target: 'https://localhost:5000'
            // },
            // '^/Board': {
            //    target: 'https://localhost:5000'
            // },
            // '^/Comment': {
            //    target: 'https://localhost:5000'
            // },
            // '^/Statistics': {
            //    target: 'https://localhost:5000'
            // },
            // '^/User': {
            //    target: 'https://localhost:4000'
            // },
            // '^/Authorize': {
            //    target: 'https://localhost:3000',
            // },

            /*Connect to local microservices via Gateway Service*/

            // '^/Project': {
            //    target: 'https://localhost:9070'
            // },
            // '^/Board': {
            //     target: 'http://localhost:9070'
            // },
            // '^/Comment': {
            //     target: 'http://localhost:9070'
            // },
            // '^/Statistics': {
            //    target: 'http://localhost:9070'
            // },
            // '^/User': {
            //     target: 'http://localhost:9070'
            // },
            // '^/Authorize': {
            //     target: 'http://localhost:9070'
            // }

            /************************************* Production *********************************/
            /*Connect to remote Microservices directly*/

            // '^/Project': {
            //    target: 'https://10.63.223.6:5000'
            // },
            //'^/Board': {
            //    target: 'https://10.63.223.6:5000'
            //},
            //'^/Comment': {
            //    target: 'https://10.63.223.6:5000'
            //},
            // '^/Statistics': {
            //    target: 'https://10.63.223.6:5000'
            // },
            //'^/User': {
            //    target: 'https://10.63.223.6:4000'
            //},
            //'^/Authorize': {
            //    target: 'https://10.63.223.6:3000'
            //}

            /*Connect to remote microservices via Gateway Service*/

            '^/Project': {
              target: 'http://techgroupdockerdc:9070'
            },
            '^/Board': {
              target: 'http://techgroupdockerdc:9070'
            },
            '^/Comment': {
              target: 'http://techgroupdockerdc:9070'
            },
            '^/Statistics': {
              target: 'http://techgroupdockerdc:9070'
            },
            '^/User': {
              target: 'http://techgroupdockerdc:9070'
            },
            '^/Authorize': {
              target: 'http://techgroupdockerdc:9070'
            }

            /*Connect to remote microservices on K8S*/

            // '^/Project': {
            //    target: 'https://10.63.207.97:30500'
            // },
            // '^/Board': {
            //    target: 'https://10.63.207.97:30500'
            // },
            // '^/Comment': {
            //    target: 'https://10.63.207.97:30500'
            // },
            // '^/Statistics': {
            //    target: 'https://10.63.207.97:30500'
            // },
            // '^/User': {
            //    target: 'https://10.63.207.97:30400'
            // },
            // '^/Authorize': {
            //    target: 'https://10.63.207.97:30300'
            // }
        },
        port: 80
    }
}