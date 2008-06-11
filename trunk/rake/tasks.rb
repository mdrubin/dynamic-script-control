require 'zip/zip'
require 'zip/zipfilesystem'

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
task :deploy_build => [:update_from_svn, "build:release", :upload_to_svn]

desc "uploads the current sources to svn"
task :upload_to_svn => [:update_from_svn] do
  msg = ENV['cm'].nil? ? "Automatic check in from rake" : ENV['cm']
  tmp_msg= "#@base_path/svn_message"
  File.open(tmp_msg, 'w+') { |f| f << msg }  
  system "svn ci -F #{tmp_msg}"  
  File.delete tmp_msg
end

desc "Updates from svn" 
task :update_from_svn do
  system "svn up" 
end

desc "Creates a zip after building the release build"
task :package => [:deploy_build, :zip_only]

desc "Creates a zip file of a release build" 
task :zip_only do
  package_files
end

def build(options={})
  cmd= "msbuild #{@base_path}/src\\DynamicScriptControl\\DynamicScriptControl.csproj"
  
  opts = options.collect{ |k, v| "#{k}=#{v}"}.join(';')
  system "#{cmd} /p:#{opts}" unless opts.nil? || opts.empty?  
end

 def package_files
   Dir.mkdir("#{@base_path}/deploy") unless File.exists? "#{@base_path}/deploy"
   version = ENV['v'].nil? ? '' : "-#{ENV['v']}"
   Zip::ZipFile.open("#{@base_path}/deploy/dynamic-script-control#{version}.zip", Zip::ZipFile::CREATE) do |zipfile|
     Dir.glob("#{@release_path}**/*").each do |file|
      zipfile.add(File.basename(file), file)
     end                    
   end                                                                                
   
 end

