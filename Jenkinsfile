pipeline {
  agent any
  environment {
    // Change to fit your needs
    BASE_VERSION='1.0.0'
    DOCKER_REPOSITORY='https://index.docker.io/v1/'
    DOCKER_REPOSITORY_SECRET='codeswifterstarter-docker-hub'
    APP_IMAGE_NAME='CHANGE_ME/codeswifterstarter-webapp'
    TEST_IMAGE_NAME='CHANGE_ME/codeswifterstarter-test'
    // Make sure to have kubectl installed and accessible through $PATH variable
    KUBECTL_CONFIG_SECRET=credentials('do-codeswifterstarter-kube-config')
    BUILD_CONFIGURATION=sh(returnStdout: true, script: (env.BRANCH_NAME == 'master' ? 'echo Release' : 'echo Staging'))
  }
  options { 
	buildDiscarder(logRotator(numToKeepStr: '10'))
	disableConcurrentBuilds()
  }
  stages {
    stage('Run unit tests') {
      steps {
        script {
          sh label:
            'Preparing environment for running tests...',
            script: '''
              echo "Workspace is $WORKSPACE"
              cd $WORKSPACE/src/CodeSwifterStarter.Application.Tests/
              docker build -f "$WORKSPACE/src/CodeSwifterStarter.Application.Tests/Dockerfile" --force-rm -t ${TEST_IMAGE_NAME} "$WORKSPACE"
            '''
        }
      }
      post {
        success {
          echo "Done unit testing"
        }
        failure {
          error 'Error running unit tests'
        }
      }
    }
    stage('Build web app') {
      when { 
        expression { return env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'staging' }
      }
      steps {
        sh label:
          'Preparing docker image...',
          script: '''
            echo "Workspace is $WORKSPACE"
            cd $WORKSPACE/src/CodeSwifterStarter.Web.Api/
            docker build -f "$WORKSPACE/src/CodeSwifterStarter.Web.Api/Dockerfile" --cache-from=${TEST_IMAGE_NAME}  --build-arg BUILD_NUMBER=${BUILD_NUMBER} --build-arg GIT_COMMIT=${GIT_COMMIT} --build-arg BUILD_CONFIGURATION=${BUILD_CONFIGURATION} --force-rm -t ${APP_IMAGE_NAME}:${BRANCH_NAME}-latest -t ${APP_IMAGE_NAME}:${BRANCH_NAME}-${BASE_VERSION}.${BUILD_NUMBER} "$WORKSPACE"
          '''
      }
      post {
        success {
          echo "Done building web app"
        }
        failure {
          error("Error building web app")
        }
      }
    }
    stage('Push docker image for the web app') {
      when { 
        expression { return env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'staging' }
      }
      steps {
        script {
          docker.withRegistry("${DOCKER_REPOSITORY}", "${DOCKER_REPOSITORY_SECRET}") {
            def latestImage = docker.image('${APP_IMAGE_NAME}:${BRANCH_NAME}-latest')
            def versionedImage = docker.image('${APP_IMAGE_NAME}:${BRANCH_NAME}-${BASE_VERSION}.${BUILD_NUMBER}')
            latestImage.push()
            versionedImage.push()
          }
        }
      }
      post {
        success {
          echo 'Docker image for the web app pushed successfully'
        }
        failure {
          error 'Error pushing docker image for the web app'
        }
      }
    }
    stage('Remove dangling docker images') {
      when { 
        expression { return env.BRANCH_NAME == 'master' || env.BRANCH_NAME == 'staging' }
      }
      steps {
        script {
          sh label:
            'Removing dangling docker images...',
            script: '''
               docker rmi --force $(docker images -f "dangling=true" -q)
               docker rmi --force $(docker images -f "reference=CHANGE_ME/codeswifterstarter-webapp*" -q)
            '''
        }
      }
      post {
        success {
          echo "Dangling images purged successfully"
        }
        failure {
          error 'Error purging dangling docker images'
        }
      }
    }
    stage('Shut down pods') {
      when { branch 'staging' }
      parallel {
        stage('Pause next step') {
          steps {
            echo "Waiting for pods to shut down..."
            sleep(time: 20, unit: "SECONDS")
          }
        }
        stage('Shutting down') {
            steps {
              script {
                sh label:
                  'Setting replicas to zero for staging environment...',
                  script: '''
                    kubectl --kubeconfig "${KUBECTL_CONFIG_SECRET}" scale deployment codeswifterstarter-app-staging --replicas=0
                '''
              }
            }
            post {
              success {
                echo 'Replica set to zero for staging environment'
              }
              failure {
                echo 'Couldn\'t set replica to zero for staging environment'
              }
            }
        }
      }
    }
    stage('Deploy application to staging') {
     when { branch 'staging' }
      steps {
        script {
          sh label:
            'Deploying to staging environment...',
            script: '''
              kubectl --kubeconfig "${KUBECTL_CONFIG_SECRET}" patch deployment codeswifterstarter-app-staging -p \"{\\"spec\\":{\\"template\\":{\\"metadata\\":{\\"annotations\\":{\\"version\\":\\"${BASE_VERSION}.${BUILD_NUMBER}\\"}},\\"spec\\":{\\"containers\\":[{\\"name\\":\\"codeswifterstarter-webapp\\",\\"image\\":\\"${APP_IMAGE_NAME}:${BRANCH_NAME}-${BASE_VERSION}.${BUILD_NUMBER}\\"}]}}}}\"
              kubectl --kubeconfig "${KUBECTL_CONFIG_SECRET}" scale deployment codeswifterstarter-app-staging --replicas=1
          '''
        }
      }
      post {
        success {
          echo 'CodeSwifterStarter deployed successfully to staging environment'
        }
        failure {
          error 'Error deploying CodeSwifterStarter to staging environment'
        }
      }
    }
    stage('Deploy application to production') {
     when { branch 'master' }
      steps {
        script {
          sh label:
            'Deploying to production environment...',
            script: '''
              kubectl --kubeconfig "${KUBECTL_CONFIG_SECRET}" patch deployment codeswifterstarter-app-production -p \"{\\"spec\\":{\\"template\\":{\\"metadata\\":{\\"annotations\\":{\\"version\\":\\"${BASE_VERSION}.${BUILD_NUMBER}\\"}},\\"spec\\":{\\"containers\\":[{\\"name\\":\\"codeswifterstarter-webapp\\",\\"image\\":\\"${APP_IMAGE_NAME}:${BRANCH_NAME}-${BASE_VERSION}.${BUILD_NUMBER}\\"}]}}}}\"
          '''
        }
      }
      post {
        success {
          echo 'CodeSwifterStarter deployed successfully to production environment'
        }
        failure {
          error 'Error deploying CodeSwifterStarter to production environment'
        }
      }
    }
  }
}