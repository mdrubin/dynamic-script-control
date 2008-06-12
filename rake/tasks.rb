require 'zip/zip'
require 'zip/zipfilesystem'
require 'fileutils'

@release_path = "#{File.dirname(__FILE__) + "\\.."}\\build\\"
@base_path = File.dirname(__FILE__) + "/.."

task :default => ["build"] do
end

desc "Build the project [DEFAULT]"
task :build do
  config = ENV['configuration'].nil? ? 'debug' : ENV['configuration']
  output_directory = ENV['output'].nil? ? "#{@base_path}\\bin\\#{config}" : ENV['output']
  output_directory = File.expand_path(output_directory)
  output_directory = "#{output_directory}\\" unless /\\$/ =~ (output_directory)
  build :configuration => config, :outdir => output_directory
end

namespace :build do 
  
  desc "Builds a release version and puts it in .\\build"
  task :release do
    build :configuration => 'release', :outdir => @release_path   
  end
end

desc "Updates from the repository, builds a release version and updates svn"
task :deploy_build => ['svn:update', "build:release", 'svn:check_in']



desc "Creates a zip after building the release build"
task :package => [:deploy_build, 'zip:release', 'zip:source' ]

namespace :zip do

  desc "Creates a zip file of a release build" 
  task :release do
    package_files @release_path, 'binaries' 
  end

  desc "Creates a zip file of a the source code"
  task :source => 'svn:export' do
    src_dep_path = "#@base_path/deploy/src"
    package_files "@base_path/deploy/src", "src"
    Dir.rm_rf(src_dep_path)
  end
end

namespace :svn do
  
  desc "uploads the current sources to svn"
  task :check_in => [:update] do
    msg = ENV['cm'].nil? ? "Automatic check in from rake" : ENV['cm']
    tmp_msg= "#@base_path/svn_message"
    File.open(tmp_msg, 'w+') { |f| f << msg }  
    system "svn ci -F #{tmp_msg}"  
    File.delete tmp_msg
  end
  
  desc "Updates from svn" 
  task :update do
    system "svn up" 
  end
  
  desc "behaves like export but from the local filesystem"
  task :local_export do
    Dir.mkdir("#@base_path/deploy") unless File.exists? "#@base_path/deploy" 
    src_dep_path = "#@base_path/deploy/src"
    Dir.mkdir(src_dep_path)
    sources = Dir.glob("#@base_path/src/**/*").collect{ |f| f unless /.svn/ =~ f }.compact!
    FileUtils.cp_r sources, src_dep_path 
    #Dir.rm_rf(src_dep_path)
  end
  
  desc "checks out the source code without the svn metadata" 
  task :export => :check_in do
    Dir.mkdir("#@base_path/deploy") unless File.exists? "#@base_path/deploy" 
    src_dep_path = "#@base_path/deploy/src"
    rm_rf(src_dep_path) if File.exist? src_dep_path
    Dir.mkdir(src_dep_path)
    system "svn export http://dynamic-script-control.googlecode.com/svn/trunk/ #{src_dep_path}"
  end
end

def build(options={})
  cmd= "msbuild #{@base_path}/src\\DynamicScriptControl\\DynamicScriptControl.csproj"
  
  opts = options.collect{ |k, v| "#{k}=#{v}"}.join(';')
  system "#{cmd} /p:#{opts}" unless opts.nil? || opts.empty?  
end

 def package_files(path, type)
   Dir.mkdir("#{@base_path}/deploy") unless File.exists? "#{@base_path}/deploy"
   version = ENV['v'].nil? ? '' : "-#{ENV['v']}"
   Zip::ZipFile.open("#{@base_path}/deploy/dynamic-script-control-#{type}#{version}.zip", Zip::ZipFile::CREATE) do |zipfile|
     Dir.glob("#{path}**/*").each do |file|
      zipfile.add(File.basename(file), file) unless /.svn/ =~ file
     end                    
   end                                                                                
   
 end

